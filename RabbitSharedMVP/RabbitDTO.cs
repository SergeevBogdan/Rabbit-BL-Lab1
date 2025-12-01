using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitSharedMVP
{
    // В RabbitSharedMVP добавляем
    public interface IDTO
    {
        int Id { get; set; }
        string Name { get; set; }
        string Breed { get; set; }
        int Age { get; set; }
        int Weight { get; set; }
    }

    // Оставляем старый RabbitDTO для обратной совместимости
    public class RabbitDTO : IDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Breed { get; set; }
        public int Age { get; set; }
        public int Weight { get; set; }
    }

    // Добавляем новый расширенный DTO
    public class RabbitExtendedDTO : RabbitDTO
    {
        public bool IsIdEditable { get; set; } = true;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public string Description { get; set; }
    }
    public class SortOperationDTO
    {
        public int SortField { get; set; }
        public bool Ascending { get; set; }

        public SortOperationDTO(int sortField, bool ascending)
        {
            SortField = sortField;
            Ascending = ascending;
        }
        public SortOperationDTO() { }
    }
}
