using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.RepresentationModel;
using YamlDotNet.Serialization;

namespace Inventory.Infrastructure.GroupVarsFiles
{
    public class InventoryFilesContext
    {
        private string _inventoryPath;

        private ILogger _logger;

        public InventoryFilesContext(ILogger<InventoryFilesContext> logger)
        {
            _logger = logger != null ? logger : throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Get all variables by fileName in the inventory path
        /// </summary>
        /// <returns></returns>
        public Task<Dictionary<String, JObject>> GetVariablesAsync()
        {
            var variables = this.GetVariables("");
            return Task.FromResult<Dictionary<String, JObject>>(variables);
        }
        public Dictionary<String, JObject> GetVariables(string inventoryPath)
        {
            _inventoryPath = !String.IsNullOrWhiteSpace(inventoryPath) ? inventoryPath : throw new ArgumentNullException(nameof(inventoryPath));
            _logger.LogInformation($"Search variables files in {_inventoryPath}");

            var directory = new DirectoryInfo(_inventoryPath);
            var files = directory.GetFiles("*.yml", SearchOption.AllDirectories);

            var variables = new Dictionary<String, JObject>();
            foreach (FileInfo file in files)
            {
                var groupName = System.IO.Path.GetFileNameWithoutExtension(file.Name);
                variables.Add(
                    key: groupName,
                    value: this.ParseFile(file.FullName)
                );
            }

            return variables;

        }


        /// <summary>
        /// Parse a single yaml file to Json Object
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private JObject ParseFile(string filePath)
        {

            //using (var input = new StreamReader(_inventoryPath))
            //{
            //    var deserializer = new DeserializerBuilder().Build();
            //    var yamlObject = deserializer.Deserialize(input);

            //    var serializer = new SerializerBuilder()
            //        .JsonCompatible()
            //        .Build();

            //    var json = serializer.Serialize(yamlObject);
            //}
            _logger.LogInformation($"Parse variable from variables file {filePath}");

            using (var input = new StreamReader(filePath))
            {
                var deserializer = new DeserializerBuilder().Build();
                var parser = new Parser(input);

                // Consume the stream start event "manually"
                parser.Consume<StreamStart>();

                var jsonObject = new JObject();

                DocumentStart dummyStart;
                DocumentEnd dummyEnd;
                while (parser.TryConsume<DocumentStart>(out dummyStart))
                {
                    var yamlObject = deserializer.Deserialize(parser);
                    var serializer = new SerializerBuilder()
                        .JsonCompatible()
                        .Build();

                    var json = serializer.Serialize(yamlObject);
                    var docJObject = JObject.Parse(json);
                    jsonObject.Merge(docJObject, new JsonMergeSettings
                    {
                        MergeArrayHandling = MergeArrayHandling.Union,
                        MergeNullValueHandling = MergeNullValueHandling.Merge,
                        PropertyNameComparison = StringComparison.InvariantCulture
                    });

                    parser.TryConsume<DocumentEnd>(out dummyEnd);
                }

                System.Diagnostics.Debug.Write(jsonObject.ToString());

                return jsonObject;

            }

        }

    }
}
