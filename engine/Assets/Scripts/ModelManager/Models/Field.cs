using System;
namespace Synthesis.ModelManager.Models
{
    public class Field : Model
    {
        public Field(string filePath)
        {
            this.gameObject = Parse.AsField(filePath);
        }
    }
}
