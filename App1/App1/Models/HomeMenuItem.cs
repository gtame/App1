using System;
using System.Collections.Generic;
using System.Text;

namespace App1.Models
{
    public enum MenuItemType
    {

        Historico,
        Acercade,
        Consumo,
        Logout
    }
    public class HomeMenuItem
    {
        public MenuItemType Id { get; set; }

        public string Title { get; set; }
    }
}
