using RabbitSharedMVP;
using System;
using System.ComponentModel;

namespace RabbitViewModels
{
    public class RabbitExtendedDTO : RabbitDTO, INotifyPropertyChanged
    {
        private bool _isIdEditable;
        private DateTime _createdDate;
        private string _description;

        public bool IsIdEditable
        {
            get => _isIdEditable;
            set
            {
                if (_isIdEditable != value)
                {
                    _isIdEditable = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateTime CreatedDate
        {
            get => _createdDate;
            set
            {
                if (_createdDate != value)
                {
                    _createdDate = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Description
        {
            get => _description;
            set
            {
                if (_description != value)
                {
                    _description = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public RabbitExtendedDTO()
        {
            IsIdEditable = true;
            CreatedDate = DateTime.Now;
        }

        public RabbitExtendedDTO(int id, string name, string breed, int age, int weight)
            : this()
        {
            Id = id;
            Name = name;
            Breed = breed;
            Age = age;
            Weight = weight;
        }

        public static RabbitExtendedDTO FromRabbitDTO(RabbitDTO dto)
        {
            if (dto == null) return null;

            return new RabbitExtendedDTO
            {
                Id = dto.Id,
                Name = dto.Name,
                Breed = dto.Breed,
                Age = dto.Age,
                Weight = dto.Weight,
                IsIdEditable = false,
                CreatedDate = DateTime.Now
            };
        }

        public RabbitDTO ToRabbitDTO()
        {
            return new RabbitDTO
            {
                Id = this.Id,
                Name = this.Name,
                Breed = this.Breed,
                Age = this.Age,
                Weight = this.Weight
            };
        }
    }

}