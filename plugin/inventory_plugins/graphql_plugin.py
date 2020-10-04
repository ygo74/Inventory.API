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
from ansible.module_utils.common._collections_compat import MutableMapping
from ansible.plugins.inventory import BaseFileInventoryPlugin

from gql import gql, Client, AIOHTTPTransport

NoneType = type(None)


class InventoryModule(BaseFileInventoryPlugin):

    NAME = 'graphql_plugin'

    def __init__(self):

        super(InventoryModule, self).__init__()

    def verify_file(self, path):

        valid = True
        return valid

    def parse(self, inventory, loader, path, cache=True):
        ''' parses the inventory file '''

        super(InventoryModule, self).parse(inventory, loader, path)
        self.set_options()

        try:

            # Select your transport with a defined url endpoint
            transport = AIOHTTPTransport(url="https://192.168.1.10:32778/graphql")

            # Create a GraphQL client using the defined transport
            client = Client(transport=transport, fetch_schema_from_transport=True)

            # Provide a GraphQL query
            query = gql(
                """
                query {
                    group(groupName: "windows 2019")
                    {
                        name
                        parents {
                            name
                        }
                        children {
                            name
                            servers {
                                hostName
                            }
                        }
                        servers {hostName}
                    }
                }
            """
            )

            # Execute the query on the transport
            data = client.execute(query)
            print(data["group"]["name"])

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


        self._parse_group(data["group"]["name"], data["group"])

        # We expect top level keys to correspond to groups, iterate over them
        # to get host, vars and subgroups (which we iterate over recursivelly)
        # if isinstance(data, MutableMapping):
        #     for group_name in data:
        #         self._parse_group(group_name, data[group_name])
        # else:
        #     raise AnsibleParserError("Invalid data from file, expected dictionary and got:\n\n%s" % to_native(data))

    def _parse_group(self, group, group_data):

        if isinstance(group_data, (MutableMapping, NoneType)):

            try:
                group = self.inventory.add_group(group)
            except AnsibleError as e:
                raise AnsibleParserError("Unable to add group %s: %s" % (group, to_text(e)))

            if group_data is not None:
                # make sure they are dicts
                for section in ['children', 'servers']:
                    if section in group_data:
                        # convert strings to dicts as these are allowed
                        if isinstance(group_data[section], string_types):
                            group_data[section] = {group_data[section]: None}

                        # if not isinstance(group_data[section], (MutableMapping, NoneType)):
                        #     raise AnsibleParserError('Invalid "%s" entry for "%s" group, requires a dictionary, found "%s" instead.' %
                        #                              (section, group, type(group_data[section])))
                if "children" in group_data:
                    for child in group_data["children"]:
                        print(child["name"])
                        subgroup = self._parse_group(child["name"], child)
                        self.inventory.add_child(group, subgroup)

                if "servers" in group_data:
                    for host_pattern in group_data["servers"]:
                        self.inventory.add_host(host_pattern["hostName"], group=group)

                if "parents" in group_data:
                    for parent in group_data["parents"]:
                        print(parent["name"])
                        parentGroup = self.inventory.add_group(parent["name"])
                        self.inventory.add_child(parent["name"], group)

        return group

    def _parse_group2(self, group, group_data):

        if isinstance(group_data, (MutableMapping, NoneType)):

            try:
                group = self.inventory.add_group(group)
            except AnsibleError as e:
                raise AnsibleParserError("Unable to add group %s: %s" % (group, to_text(e)))

            if group_data is not None:
                # make sure they are dicts
                for section in ['vars', 'children', 'hosts']:
                    if section in group_data:
                        # convert strings to dicts as these are allowed
                        if isinstance(group_data[section], string_types):
                            group_data[section] = {group_data[section]: None}

                        if not isinstance(group_data[section], (MutableMapping, NoneType)):
                            raise AnsibleParserError('Invalid "%s" entry for "%s" group, requires a dictionary, found "%s" instead.' %
                                                     (section, group, type(group_data[section])))

                for key in group_data:

                    if not isinstance(group_data[key], (MutableMapping, NoneType)):
                        self.display.warning('Skipping key (%s) in group (%s) as it is not a mapping, it is a %s' % (key, group, type(group_data[key])))
                        continue

                    if isinstance(group_data[key], NoneType):
                        self.display.vvv('Skipping empty key (%s) in group (%s)' % (key, group))
                    elif key == 'vars':
                        for var in group_data[key]:
                            self.inventory.set_variable(group, var, group_data[key][var])
                    elif key == 'children':
                        for subgroup in group_data[key]:
                            subgroup = self._parse_group(subgroup, group_data[key][subgroup])
                            self.inventory.add_child(group, subgroup)

                    elif key == 'hosts':
                        for host_pattern in group_data[key]:
                            hosts, port = self._parse_host(host_pattern)
                            self._populate_host_vars(hosts, group_data[key][host_pattern] or {}, group, port)
                    else:
                        self.display.warning('Skipping unexpected key (%s) in group (%s), only "vars", "children" and "hosts" are valid' % (key, group))

        else:
            self.display.warning("Skipping '%s' as this is not a valid group definition" % group)

        return group

    def _parse_host(self, host_pattern):
        '''
        Each host key can be a pattern, try to process it and add variables as needed
        '''
        (hostnames, port) = self._expand_hostpattern(host_pattern)

        return hostnames, port
