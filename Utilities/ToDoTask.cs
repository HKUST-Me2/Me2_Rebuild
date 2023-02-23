using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Bot.Schema;

namespace Microsoft.BotBuilderSamples.Utilities
{
    public class ToDoTask
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        public string Task {get; set;}
        public string Year { get; set; }
        public string Season { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public string DateAdditional { get; set; }
        public string Place { get; set; }
        public string PlaceAdditional { get; set; }
        public string Eyewitness { get; set; }
        public string Interact { get; set; }
        public string EyewitnessAdditional { get; set; }
        public string ToldOthers { get; set; }
        public string ToldOthersAdditional { get; set; }
        public string Offender { get; set; }
        public string multiplechoice{ get; set; }
        public string remember { get; set; }
        public string numOfOffender { get; set; }
        public string nameOfOffender { get; set; }
        public string infoOfOffender { get; set; }
        public string infoOfThesePeople { get; set; }
        public List<Attachment> attachemntDoc{ get; set; }


        
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
