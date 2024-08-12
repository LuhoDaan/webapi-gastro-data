using System;

namespace GastroApi.Models
{
    public class GastroItemDocument
    {
        public long Id { get; set; }
        public string DescriptionName { get; set; }
        public string Ingredients { get; set; }
        public string Recipe { get; set; }
        public int? TimeToPrepare { get; set; }
    }
}