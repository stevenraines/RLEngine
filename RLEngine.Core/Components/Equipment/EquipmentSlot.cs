using System.Text.Json;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations.Schema;
namespace RLEngine.Core.Components
{

    public class EquipmentSlot : IEquipmentSlot
    {

        public string Name { get; set; }

        protected EquipmentSlot()
        {
        }

        public EquipmentSlot(string name)
        {
            Name = name;
        }


    }
}