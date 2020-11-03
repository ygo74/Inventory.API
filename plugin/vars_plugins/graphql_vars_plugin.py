from __future__ import (absolute_import, division, print_function)
__metaclass__ = type

DOCUMENTATION = '''

'''
EXAMPLES = '''
'''

import os
import configparser
from gql import gql, Client, AIOHTTPTransport

from ansible import constants as C
from ansible.errors import AnsibleParserError
from ansible.module_utils._text import to_bytes, to_native, to_text
from ansible.plugins.vars import BaseVarsPlugin
from ansible.inventory.host import Host
from ansible.inventory.group import Group
from ansible.utils.vars import combine_vars
from ansible.utils.display import Display
from ansible.module_utils.common._collections_compat import Mapping, MutableMapping

display = Display()

FOUND = {}

OPTIONS = {}

class VarsModule(BaseVarsPlugin):

    REQUIRES_WHITELIST = True

    NAME = 'graphql_vars_plugin'

    def __init__(self):

        super(VarsModule, self).__init__()
        self.display = display

        # self._options = self.loader.load_from_file('inventories/poc/graphql_plugin.yaml',cache=False)
        # print(self._options)

        self.api_server = ""
        self.api_token  = ""

    def verify_file(self, path):
        ''' Verify if file is usable by this plugin, base does minimal accessibility check
            :arg path: a string that was passed as an inventory source,
                 it normally is a path to a config file, but this is not a requirement,
                 it can also be parsed itself as the inventory data to process.
                 So only call this base class if you expect it to be a file.
        '''

        try:
            valid = False
            b_path = to_bytes(path, errors='surrogate_or_strict')
            if (os.path.exists(b_path) and os.access(b_path, os.R_OK)):
                valid = True
            else:
                self.display.vvv('Skipping due to vars plugin source not existing or not being readable by the current user')
            return valid
        except Exception as e:
            raise AnsibleParserError(e)

    def _read_config_data(self, path):
        ''' validate config and set options as appropriate
            :arg path: path to common yaml format config file for this plugin
        '''

        config = {}
        try:
            # avoid loader cache so meta: refresh_inventory can pick up config changes
            # if we read more than once, fs cache should be good enough
            config = self.loader.load_from_file(path, cache=False)
        except Exception as e:
            raise AnsibleParserError(to_native(e))

        # a plugin can be loaded via many different names with redirection- if so, we want to accept any of those names
        # valid_names = getattr(self, '_redirected_names') or [self.NAME]

        if not config:
            # no data
            raise AnsibleParserError("%s is empty" % (to_native(path)))
        elif not isinstance(config, Mapping):
            # configs are dictionaries
            raise AnsibleParserError('vars plugin source has invalid structure, it should be a dictionary, got: %s' % type(config))

        return config


    def parse_config_file(self, path):
        print(path)
        config = self._read_config_data(path)
        OPTIONS['api_server'] = config['api_server']
        OPTIONS['api_token']  = config['api_token']



    def get_vars(self, loader, path, entities, cache=True):
        ''' parses the inventory file '''

        if not isinstance(entities, list):
            entities = [entities]

        super(VarsModule, self).get_vars(loader, path, entities)

        if 'api_server' not in OPTIONS.keys():
            self.loader = loader
            config_file_path = path + "/graphql_plugin.yaml"
            self.display.v('Load vars plugin configuration file {}'.format(config_file_path))

            if self.verify_file(config_file_path):
                self.parse_config_file(config_file_path)
            else:
                return {}

        self.api_server = OPTIONS['api_server']
        self.api_token  = OPTIONS['api_token']

        data = {}
        for entity in entities:
            if isinstance(entity, Host):
                subdir = 'host_vars'
            elif isinstance(entity, Group):
                subdir = 'group_vars'
            else:
                raise AnsibleParserError("Supplied entity must be Host or Group, got %s instead" % (type(entity)))

            if isinstance(entity, Group):
                key = "Group_%s" % entity.name
                if cache and key in FOUND:
                    self.display.v('Load vars from cache')
                    new_data = FOUND[key]
                else:
                    self.display.v('Load vars from graphql api {}'.format(self.api_server))
                    try:

                        # Select your transport with a defined url endpoint
                        transport = AIOHTTPTransport(url="https://{}/graphql".format(self.api_server))

                        # Create a GraphQL client using the defined transport
                        client = Client(transport=transport, fetch_schema_from_transport=True)

                        # Provide a GraphQL query
                        query = gql(
                            '''
                            query {
                                groupByName(groupName: "''' + entity.name + '''")
                                {
                                    ansible_group_name
                                    variables
                                }
                            }
                        '''
                        )

                        # Execute the query on the transport
                        new_data = client.execute(query)

                        if new_data["groupByName"] != None:
                            new_data = new_data["groupByName"]["variables"]
                        else:
                            new_data = {}

                        FOUND[key] = new_data

                    except Exception as e:
                        raise AnsibleParserError(e)


                data = combine_vars(data, new_data)

        return data
