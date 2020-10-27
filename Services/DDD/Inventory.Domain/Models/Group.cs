using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;

namespace Inventory.Domain.Models
{
    public class Group : Entity
    {
        public int GroupId { get; private set; }

        [Required]
        public String Name { get; private set; }

        [Required]
        public String AnsibleGroupName { get; private set; }

        // Many to Many server Groups
        private List<ServerGroup> _serverGroups = new List<ServerGroup>();
        public ICollection<ServerGroup> ServerGroups => _serverGroups.AsReadOnly();

        //self reference
        public Group Parent { get; private set; }
        public int? ParentId { get; private set; }

        private List<Group> _children = new List<Group>();
        public IEnumerable<Group> Children => _children.AsReadOnly();

        protected Group() { }

        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="ansibleGroupName"></param>
        public Group(String name, String ansibleGroupName = null)
        {
            Name = !String.IsNullOrEmpty(name) ? name.ToLower() : throw new ArgumentNullException(nameof(name));
            AnsibleGroupName = !String.IsNullOrEmpty(ansibleGroupName) ? ansibleGroupName.ToLower() : this.Name;
        }


        public void AddSubGroups(Group group)
        {
            var existingGroup = _children.SingleOrDefault(grp => grp.Name == group.Name);
            if (null == existingGroup)
            {
                _children.Add(group);
            }
        }

        public void AddServers(List<Server> servers)
        {
            servers.ForEach(srv => this.AddServer(srv));
        }

        public void AddServer(Server server)
        {
            var serverGroup = new ServerGroup()
            {
                Group = this,
                Server = server
            };
            _serverGroups.Add(serverGroup);
        }

    }
}
