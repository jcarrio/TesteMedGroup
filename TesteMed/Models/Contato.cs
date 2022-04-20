using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TesteMed.Models
{
    public enum sexo
    {
        [Display(Name = "")]
        NaoInformado = 0,
        [Display(Name = "Masculino")]
        Masculino = 1,
        [Display(Name = "Feminino")]
        Feminino = 2,
        [Display(Name = "Outros")]
        Outros = 3
    }
    public class Contato
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Nome do contato")]
        [Required(ErrorMessage = "Campo obrigatório")]
        public string Nome { get; set; }
        [Display(Name = "Data de Nascimento")]
        [Required(ErrorMessage = "Campo obrigatório")]
        [DataType(DataType.Date)]
        [CustomValidation(typeof(Contato), "Validate", ErrorMessage = "Contato precisa ser maior de idade")]
        public DateTime DataNasc { get; set; }
        [Range(1, 3, ErrorMessage = "Campo obrigatório")]
        [EnumDataType(typeof(sexo))]
        public sexo Sexo { get; set; }
        public bool Ativo { get; set; }
        [NotMapped]
        public int Idade {
            get
            {
                int age = DateTime.Now.Year - DataNasc.Year;
                if (DateTime.Now < DataNasc.AddYears(age))
                    age--;
                return age;
//                return (DateTime.Now.Month > DataNasc.Month || (DateTime.Now.Month == DataNasc.Month && DateTime.Now.Day >= DataNasc.Day)) ? DateTime.Now.Year - DataNasc.Year : DateTime.Now.Year - DataNasc.Year - 1;
            }
        }
        public static ValidationResult Validate(DateTime date, ValidationContext context)
        {
            return (date.AddYears(18) < DateTime.Now) ? ValidationResult.Success : new ValidationResult(null);
        }
        public Contato()
        {
            Ativo = true;
        }
    }
}
