using System;

namespace SLHub.Addons
{
    public class AddonConfig : Attribute
    {
        public AddonConfig(string section) => Section = section;

        public string Section { get; set; }
    }
}
