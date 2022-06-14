namespace Entities.Enums
{
    public class MessagesEnum
    {
        public const string InvalidModel = "Modelo inválido";
        public const string HttpStateNotFound = "404 - Not Found";
        public const string HttpStateOk = "200 - Ok";
        public const string HttpStateAccepted = "202 - Accepted";
        public const string HttpStateBadRequest = "400 - Bad Request";
        public const string HttpStateUnauthorized = "401 - Unauthorized";
        public const string DbUpdateException = "Hubo un error al actualizar el modelo";
        public const string DbError = "Error en el motor de Base de Datos";
        public const string OwnError = "Error controlado o regla de negocio";
        public const string FatalError = "Error fatal";
        public const string AdminError = "Error fatal, por favor contactese con el administrador";
        public const string EmailError = "No se encontró una cuenta asociada a este correo electrónico";
        public const string EmailSuccess = "Le enviamos un email con instrucciones para restablecer su contraseña. Si no lo encuentra en su bandeja de entrada, revise la carpeta de correo no deseado.";
        public const string PasswordTokenError = "Este enlace para cambiar la contraseña no es válido";
        public const string PasswordError = "Contraseña inválida";
        public const string EmailAlreadyUsed = "dirección de correo electrónico";
        public const string NickNameAlreadyUsed = "nickname";
        public const string DocumentIdAlreadyUsed = "número de identificación";
        public const string PropertyNotEmpty = "El campo no puede ser vacío.";
        public const string PropertyNotNull = "El campo no puede ser nulo.";
        public const string PropertyNotGreaterTo250 = "El valor ingresado no puede tener más de 250 caracteres.";
        public const string PropertyNotGreaterTo200 = "El valor ingresado no puede tener más de 200 caracteres.";
        public const string PropertyNotGreaterTo100 = "El valor ingresado no puede tener más de 100 caracteres.";
        public const string PropertyNotGreaterTo80 = "El valor ingresado no puede tener más de 80 caracteres.";
        public const string PropertyNotGreaterTo50 = "El valor ingresado no puede tener más de 50 caracteres.";
        public const string PropertyNotGreaterTo20 = "El valor ingresadono puede tener más de 20 caracteres.";
        public const string PropertyNotGreaterTo10 = "El valor ingresado no puede tener más de 10 caracteres.";
        public const string PropertyNotGreaterTo11 = "El valor ingresado no puede tener más de 11 caracteres.";
        public const string PropertyNotGreaterTo2 = "El valor ingresado no puede tener más de 2 caracteres.";
        public const string IsNotValidDate = "El valor ingresado debe ser una fecha válida.";
        public const string IsBoolean = "El valor ingresado debe ser Booleano.";
        public const string CreateDetailPredictionError = "Error: Could not create detail predictions";
        public const string UpdatePredictionError = "Error: Could not update prediction";
        public const string CreateRequestServiceDetailsError = "Error: Could not create RequestServiceDetails";
        public const string ConsumeConvertionServiceError = "Error Consume Services convertion or prediction";
        public const string OtpError = "El código ingresado no corresponde con el enviado.";
        public const string OtpTimeError = "El´códgio se encuentra vencido por favor solicitarlo nuevamente.";
    }
}