# Copyright (c) 2017 Ansible Project
# GNU General Public License v3.0+ (see COPYING or https://www.gnu.org/licenses/gpl-3.0.txt)

from __future__ import (absolute_import, division, print_function)
__metaclass__ = type

DOCUMENTATION = '''

'''
EXAMPLES = '''
'''

import os

from ansible.errors import AnsibleError, AnsibleParserError
from ansible.module_utils.six import string_types
from ansible.module_utils._text import to_native, to_text
from ansible.module_utils.common._collections_compat import Mapping, MutableMapping
from ansible.plugins.inventory import BaseInventoryPlugin, Constructable, Cacheable
from ansible.utils.vars import combine_vars

from gql import gql, Client, AIOHTTPTransport

NoneType = type(None)


class InventoryModule(BaseInventoryPlugin, Constructable, Cacheable):

    NAME = 'graphql_plugin'

    def __init__(self):

        super(InventoryModule, self).__init__()
        self.api_server = ""
        self.api_token = ""
        self.main_group = ""
        self.environment = ""


    def verify_file(self, path):

        valid = False
        if super(InventoryModule, self).verify_file(path):
            # base class verifies that file exists and is readable by current user
            if path.endswith(('graphql_plugin.yaml', 'graphql_plugin.yml')):
                valid = True
        return valid

    def parse(self, inventory, loader, path, cache=True):
        ''' parses the inventory file '''

        super(InventoryModule, self).parse(inventory, loader, path, cache)

        self._options = self._read_config_data(path)
        # print(self._options)

        self.api_server = self.get_option('api_server')
        self.api_token  = self.get_option('api_token')
        self.main_group = self.get_option('main_group')
        self.environment = self.get_option('environment')

        try:

            # Select your transport with a defined url endpoint
            transport = AIOHTTPTransport(url="https://{}/graphql".format(self.api_server))

            # Create a GraphQL client using the defined transport
            client = Client(transport=transport, fetch_schema_from_transport=True)

            # Provide a GraphQL query
            query = gql(
                '''
                query inventory {
                    inventory(groupName: "''' + self.main_group + '''", environment: "''' + self.environment + '''"){
                        groups
                        {
                            ansible_group_name
                            variables
                            parent
                            {
                                ansible_group_name
                            }
                        }
                        servers
                        {
                            hostname
                            variables
                            group_names
                        }
                    }
                }

            '''
            )

            # Execute the query on the transport
            data = client.execute(query)
            # print(data["inventory'"]["servers"])
            # print("hello")

            # self.inventory.add_group(data["group"]["name"])
            # self.inventory.add_host("test")
        except Exception as e:
            raise AnsibleParserError(e)

        if not data:
            raise AnsibleParserError('Parsed empty Graphql requests')
        elif not isinstance(data, MutableMapping):
            raise AnsibleParserError('YAML inventory has invalid structure, it should be a dictionary, got: %s' % type(data))
        elif data.get('plugin'):
            raise AnsibleParserError('Plugin configuration YAML file, not YAML inventory')


        # self._parse_group(data["groupByName"]["ansible_group_name"], data["groupByName"])
        self._parse_servers(data["inventory"]["servers"], data["inventory"]["groups"])

        # We expect top level keys to correspond to groups, iterate over them
        # to get host, vars and subgroups (which we iterate over recursivelly)
        # if isinstance(data, MutableMapping):
        #     for group_name in data:
        #         self._parse_group(group_name, data[group_name])
        # else:
        #     raise AnsibleParserError("Invalid data from file, expected dictionary and got:\n\n%s" % to_native(data))
    def _parse_servers(self, servers, groups):

        # Create groups
        if isinstance(groups, list):
            # Create all groups
            for group in groups:
                groupName = group["ansible_group_name"]
                try:
                    self.inventory.add_group(groupName)
                except AnsibleError as e:
                    raise AnsibleParserError("Unable to add group %s: %s" % (group, to_text(e)))

                if "variables" in group.keys() and isinstance(group["variables"], dict):
                    variables = group["variables"]
                    for k in variables:
                        self.inventory.set_variable(groupName, k, variables[k])

            # Create relation ships
            for group in groups:
                if "parent" in group.keys() and group["parent"] != None:
                    groupName = group["ansible_group_name"]
                    parentGroupName = group["parent"]["ansible_group_name"]

                    if parentGroupName in self.inventory.groups.keys():
                        self.inventory.add_child( parentGroupName, groupName )


        # Create Servers
        if isinstance(servers, list):
            for server in servers:
                try:
                    hostName = server["hostname"]
                    host = self.inventory.add_host(hostName)

                    # link server to groups
                    for group in server["group_names"]:
                        if group in self.inventory.groups.keys():
                            self.inventory.add_child( group, host )

                    # Add hostvars
                    if "variables" in server.keys() and isinstance(server["variables"], dict):
                        variables = server["variables"]
                        for k in variables:
                            self.inventory.set_variable(hostName, k, variables[k])


                except AnsibleError as e:
                    raise AnsibleParserError("Unable to add server %s: %s" % (server, to_text(e)))

