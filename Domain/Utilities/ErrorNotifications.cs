namespace Domain.Utilities
{
    public static class ErrorNotifications
    {   
        //Não encontrado
        public const string MOTORCYCLE_NOT_FOUND = "Moto não encontrada";
        public const string ORDER_NOT_FOUND = "Pedido não encontrado";
        public const string COURIER_NOT_FOUND = "Entregador não encontrado";
        public const string RENTAL_NOT_FOUND = "Locação não encontrada";
        public const string NO_MOTORCYCLES_AVAILABLE = "Nenhuma moto disponivel";

        //Validação
        public const string LICENSE_PLATE_USED = "Placa já utilizada";
        public const string CNH_USED = "CNH já utilizada";
        public const string CNPJ_USED = "CNPJ já utilizado";
        public const string IMAGE_INVALID_TYPE = "A imagem deve ser do tipo png ou bmp";

        //Permissão
        public const string MOTORCYCLE_WITH_RENTALS = "Moto já alugada anteriormente";
        public const string COURIER_RENTAL_ACTIVE = "Entregador já possui uma locação ativa";
        public const string COURIER_WITHOUT_CNHTYPE_A = "Entregador não possui categoria A";
        public const string RENTAL_COURIER_NOT_AUTHORIZED = "Locação não pertence ao entregador";
        public const string COURIER_NOT_NOTIFIED = "Entregador não recebeu notificação";
        public const string ORDER_UNAVAILABLE = "Pedido não está com status Disponivel";
        public const string ORDER_COURIER_NOT_AUTHORIZED = "Pedido não pertence ao entregador";
        public const string ORDER_NOT_ACCEPTED_OR_DELIVERED = "Pedido ainda não foi aceito ou já foi entregue";
        public const string RENTAL_INACTIVE = "A locação já foi finalizada";

        public const string NECESSARY_SEND_IMAGE = "Necessário enviar uma imagem";
    }

}
