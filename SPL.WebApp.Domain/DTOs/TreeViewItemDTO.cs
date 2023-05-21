namespace SPL.WebApp.Domain.DTOs
{
    using System.Collections.Generic;

    public class TreeViewItemDTO
    {
        public string id { get; set; }
        public string value { get; set; }
        public string text { get; set; }
        public string spriteCssClass { get; set; }
        public string imageUrl { get; set; }
        public bool expanded { get; set; }
        public bool hasChildren { get; set; }
        public bool? status { get; set; }
        public IEnumerable<TreeViewItemDTO> items { get; set; }

        public TreeViewItemDTO Clone()
        {
            TreeViewItemDTO clone = new TreeViewItemDTO
            {
                id = this.id,
                imageUrl = this.imageUrl,
                spriteCssClass = this.spriteCssClass,
                text = this.text,
                expanded = this.expanded,
                hasChildren = this.hasChildren
            };
            return clone;
        }
    }
}
