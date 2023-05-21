namespace SPL.WebApp.ViewModels.ETD
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public class RddeViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Requerido")]
        [DisplayName("No. Serie")]
        public string NoSerie { get; set; }
        #region TOR
        [Required(ErrorMessage = "Requerido")]
        [RegularExpression(@"^([0-9]{0,2})$", ErrorMessage = "El valor límite de Top Oil Rise debe ser numérico mayor a cero considerando 2 enteros sin decimales")]
        [Range(1, 99, ErrorMessage = "El campo debe ser numérico mayor a cero considerando 2 enteros y sin decimales")]
        public int TORLimite { get; set; }
        [RegularExpression(@"^(([0-9]{0,2})|([0-9]{0,2}\.[0-9]{1}))$", ErrorMessage = "El valor de la hoja de enfriamiento de Top Oil Rise debe ser numérico mayor a cero considerando 2 enteros con 1 decimal")]
        [Required(ErrorMessage = "Requerido")]
        [Range(0.1, 99.9, ErrorMessage = "El campo debe ser numérico mayor a cero considerando 2 enteros y 1 decimal")]
        public decimal TORHoja { get; set; }
        #endregion

        #region AOR
        [Required(ErrorMessage = "De no poseer valor debe ser 0")]
        [RegularExpression(@"^([0-9]{0,2})$", ErrorMessage = "El valor límite de Average Oil Rise debe ser numérico mayor a cero considerando 2 enteros sin decimales")]
        public int AORLimite { get; set; }
        [Required(ErrorMessage = "Requerido")]
        [RegularExpression(@"^(([0-9]{0,2})|([0-9]{0,2}\.[0-9]{1}))$", ErrorMessage = "El valor de la hoja de enfriamiento de Average Oil Rise debe ser numérico mayor a cero considerando 2 enteros con 1 decimal")]
        [Range(0.1, 99.9, ErrorMessage = "El campo debe ser numérico mayor a cero considerando 2 enteros y 1 decimal")]
        public decimal AORHoja { get; set; }
        #endregion

        #region Gradiente AT
        [Required(ErrorMessage = "De no poseer valor debe ser 0")]
        [RegularExpression(@"^(([0-9]{0,2})|([0-9]{0,2}\.[0-9]{1,2}))$", ErrorMessage = "El valor límite de Gradiente para alta tensión debe ser numérico mayor a cero considerando 2 enteros con 2 decimales")]
        public decimal GATLimite { get; set; }
        [RegularExpression(@"^(([0-9]{0,2})|([0-9]{0,2}\.[0-9]{1,2}))$", ErrorMessage = "El valor de la hoja de enfriamiento de Gradiente para alta tensión debe ser numérico mayor a cero considerando 2 enteros con 2 decimales")]
        [Required(ErrorMessage = "De no poseer valor debe ser 0")]
        public decimal GATHoja { get; set; }
        #endregion

        #region Gradiente BT
        [Required(ErrorMessage = "De no poseer valor debe ser 0")]
        [RegularExpression(@"^(([0-9]{0,2})|([0-9]{0,2}\.[0-9]{1,2}))$", ErrorMessage = "El valor límite de Gradiente para baja tensión debe ser numérico mayor a cero considerando 2 enteros con 2 decimales")]
        public decimal GBTLimite { get; set; }
        [Required(ErrorMessage = "De no poseer valor debe ser 0")]
        [RegularExpression(@"^(([0-9]{0,2})|([0-9]{0,2}\.[0-9]{1,2}))$", ErrorMessage = "El valor de la hoja de enfriamiento de Gradiente para baja tensión debe ser numérico mayor a cero considerando 2 enteros con 2 decimales")]
        public decimal GBTHoja { get; set; }
        #endregion

        #region Gradiente TER
        [Required(ErrorMessage = "De no poseer valor debe ser 0")]
        [RegularExpression(@"^(([0-9]{0,2})|([0-9]{0,2}\.[0-9]{1,2}))$", ErrorMessage = "El valor límite de Gradiente para terciario debe ser numérico mayor a cero considerando 2 enteros con 2 decimales")]
        public decimal GTERLimite { get; set; }
        [Required(ErrorMessage = "De no poseer valor debe ser 0")]
        [RegularExpression(@"^(([0-9]{0,2})|([0-9]{0,2}\.[0-9]{1,2}))$", ErrorMessage = "El valor de la hoja de enfriamiento de Gradiente para terciario debe ser numérico mayor a cero considerando 2 enteros con 2 decimales")]
        public decimal GTERHoja { get; set; }
        #endregion

        #region AWR AT
        [Required(ErrorMessage = "De no poseer valor debe ser 0")]
        [RegularExpression(@"^([0-9]{0,2})$", ErrorMessage = "El valor límite de Average Winding Rise para alta tensión debe ser numérico mayor a cero considerando 2 enteros sin decimales")]
        public int AATLimite { get; set; }
        [Required(ErrorMessage = "De no poseer valor debe ser 0")]
        [RegularExpression(@"^(([0-9]{0,2})|([0-9]{0,2}\.[0-9]{1}))$", ErrorMessage = "El valor de la hoja de enfriamiento de Average Winding Rise para alta tensión debe ser numérico mayor a cero considerando 2 enteros con 1 decimal")]
        public decimal AATHoja { get; set; }
        #endregion

        #region AWR BT
        [Required(ErrorMessage = "De no poseer valor debe ser 0")]
        [RegularExpression(@"^([0-9]{0,2})$", ErrorMessage = "El valor límite de Average Winding Rise para baja tensión debe ser numérico mayor a cero considerando 2 enteros sin decimales")]
        public int ABTLimite { get; set; }
        [Required(ErrorMessage = "De no poseer valor debe ser 0")]
        [RegularExpression(@"^(([0-9]{0,2})|([0-9]{0,2}\.[0-9]{1}))$", ErrorMessage = "El valor de la hoja de enfriamiento de Average Winding Rise para baja tensión debe ser numérico mayor a cero considerando 2 enteros con 1 decimal")]
        public decimal ABTHoja { get; set; }
        #endregion

        #region AWR TER
        [Required(ErrorMessage = "De no poseer valor debe ser 0")]
        [RegularExpression(@"^([0-9]{0,2})$", ErrorMessage = "El valor límite de Average Winding Rise para terciario debe ser numérico mayor a cero considerando 2 enteros sin decimales")]
        public int ATERLimite { get; set; }
        [Required(ErrorMessage = "De no poseer valor debe ser 0")]
        [RegularExpression(@"^(([0-9]{0,2})|([0-9]{0,2}\.[0-9]{1}))$", ErrorMessage = "El valor de la hoja de enfriamiento de Average Winding Rise para terciario debe ser numérico mayor a cero considerando 2 enteros con 1 decimal")]
        public decimal ATERHoja { get; set; }
        #endregion

        #region HSR AT
        [Required(ErrorMessage = "De no poseer valor debe ser 0")]
        [RegularExpression(@"^([0-9]{0,3})$", ErrorMessage = "El valor límite de Hottest Spot Rise para alta tensión debe ser numérico mayor a cero considerando 3 enteros sin decimales")]
        public int HATLimite { get; set; }
        [Required(ErrorMessage = "De no poseer valor debe ser 0")]
        [RegularExpression(@"^(([0-9]{0,3})|([0-9]{0,3}\.[0-9]{1}))$", ErrorMessage = "El valor de la hoja de enfriamiento de Hottest Spot Rise para alta tensión debe ser numérico mayor a cero considerando 3 enteros con 1 decimal")]
        public decimal HATHoja { get; set; }
        #endregion

        #region HSR BT
        [Required(ErrorMessage = "De no poseer valor debe ser 0")]
        [RegularExpression(@"^([0-9]{0,3})$", ErrorMessage = "El valor límite de Hottest Spot Rise para baja tensión debe ser numérico mayor a cero considerando 3 enteros sin decimales")]
        public int HBTLimite { get; set; }
        [Required(ErrorMessage = "De no poseer valor debe ser 0")]
        [RegularExpression(@"^(([0-9]{0,3})|([0-9]{0,3}\.[0-9]{1}))$", ErrorMessage = "El valor de la hoja de enfriamiento de Hottest Spot Rise para baja tensión debe ser numérico mayor a cero considerando 3 enteros con 1 decimal")]
        public decimal HBTHoja { get; set; }
        #endregion

        #region HSR TER
        [Required(ErrorMessage = "De no poseer valor debe ser 0")]
        [RegularExpression(@"^([0-9]{0,3})$", ErrorMessage = "El valor límite de Hottest Spot Rise para terciario debe ser numérico mayor a cero considerando 3 enteros sin decimales")]
        public int HTERLimite { get; set; }
        [Required(ErrorMessage = "De no poseer valor debe ser 0")]
        [RegularExpression(@"^(([0-9]{0,3})|([0-9]{0,3}\.[0-9]{1}))$", ErrorMessage = "El valor de la hoja de enfriamiento de Hottest Spot Rise para terciario debe ser numérico mayor a cero considerando 3 enteros con 1 decimal")]
        public decimal HTERHoja { get; set; }
        #endregion

        #region Hoja de Enfriamiento
        [RegularExpression(@"^(([0-9]{0,2})|([0-9]{0,2}\.[0-9]{1}))$", ErrorMessage = "Constante tiempo trafo en horas debe ser numérico mayor a cero considerando 2 enteros con 1 decimal")]
        [Required(ErrorMessage = "Requerido")]
        [Range(0.1, 99.9, ErrorMessage = "El campo debe ser numérico mayor a cero considerando 2 enteros y 1 decimal")]
        public decimal CteTiempo { get; set; }

        [RegularExpression(@"^([0-9]{0,2})$", ErrorMessage = "Ambiente constante en horas debe ser numérico mayor a cero considerando 2 enteros sin decimales")]
        [Required(ErrorMessage = "Requerido")]
        [Range(1, 99, ErrorMessage = "El campo debe ser numérico mayor a cero considerando 2 enteros y sin decimal")]
        public int CteAmbiente { get; set; }

        [RegularExpression(@"^(([0-9]{0,2})|([0-9]{0,2}\.[0-9]{1}))$", ErrorMessage = "Bottom Oil Rise debe ser numérico mayor a cero considerando 2 enteros con 1 decimal")]
        [Required(ErrorMessage = "Requerido")]
        [Range(0.1, 99.9, ErrorMessage = "El campo debe ser numérico mayor a cero considerando 2 enteros y 1 decimal")]
        public decimal BOR { get; set; }

        [Required(ErrorMessage = "Requerido")]
        [RegularExpression(@"^(([0-9]{0,4})|([0-9]{0,4}\.[0-9]{1,3}))$", ErrorMessage = "kW Diseño debe ser numérico mayor a cero considerando 4 enteros con 3 decimales")]
        [Range(0.001, 9999.999, ErrorMessage = "El campo debe ser numérico mayor a cero considerando 4 enteros y 3 decimal")]
        public decimal KWDiseno { get; set; }

        [RegularExpression(@"^([0-9]{0,2})$", ErrorMessage = "TOI debe ser numérico mayor a cero considerando 2 enteros sin decimales")]
        [Required(ErrorMessage = "Requerido")]
        [Range(1, 99, ErrorMessage = "El campo debe ser numérico mayor a cero considerando 2 enteros y sin decimal")]
        public int TOI { get; set; }

        [Required(ErrorMessage = "Requerido")]
        [RegularExpression(@"^([0-9]{0,2})$", ErrorMessage = "Límite TOI debe ser numérico mayor a cero considerando 2 enteros sin decimales")]
        [Range(1, 99, ErrorMessage = "El campo debe ser numérico mayor a cero considerando 2 enteros y sin decimal")]
        public int TOILimite { get; set; }
        #endregion

        #region requeridos
        public bool AT { get; set; }
        public bool BT { get; set; }
        public bool TER { get; set; }
        #endregion

    }

    public class RequiredIfAttribute : ValidationAttribute
    {
        private readonly RequiredAttribute _innerAttribute = new();
        public string _dependentProperty { get; set; }
        public object _targetValue { get; set; }

        public RequiredIfAttribute(string dependentProperty, object targetValue)
        {
            _dependentProperty = dependentProperty;
            _targetValue = targetValue;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            System.Reflection.PropertyInfo field = validationContext.ObjectType.GetProperty(_dependentProperty);
            if (field != null)
            {
                object dependentValue = field.GetValue(validationContext.ObjectInstance, null);
                if ((dependentValue == null && _targetValue == null) || dependentValue.Equals(_targetValue))
                {
                    if (!_innerAttribute.IsValid(value))
                    {
                        string name = validationContext.DisplayName;
                        string specificErrorMessage = ErrorMessage;
                        if (specificErrorMessage.Length < 1)
                            specificErrorMessage = $"{name} es requerido.";

                        return new ValidationResult(specificErrorMessage, new[] { validationContext.MemberName });
                    }
                }
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult(FormatErrorMessage(_dependentProperty));
            }
        }
    }
}
