using Dominio.Entities;
using Dominio.Interfaces.Notification;

namespace Dominio.Utilities
{
    public static class ErrorNotifications
    {   
        //Não encontrado
        public const string MOTO_NAO_ENCONTRADA = "Moto não encontrada";
        public const string PEDIDO_NAO_ENCONTRADO = "Pedido não encontrado";
        public const string ENTREGADOR_NAO_ENCONTRADO = "Entregador não encontrado";
        public const string LOCACAO_NAO_ENCONTRADA = "Locação não encontrada";
        public const string NENHUMA_MOTO_DISPONIVEL = "Nenhuma moto disponivel";

        //Validação
        public const string PLACA_UTILIZADA = "Placa já utilizada";
        public const string CNH_UTILIZADA = "CNH já utilizada";
        public const string CNPJ_UTILIZADO = "CNPJ já utilizado";
        public const string IMAGEM_FORMATO_INVALIDO = "A imagem deve ser do tipo png ou bmp";

        //Permissão
        public const string MOTO_POSSUI_LOCACOES = "Moto já alugada anteriormente";
        public const string ENTREGADOR_LOCACAO_ATIVA = "Entregador já possui uma locação ativa";
        public const string ENTREGADOR_SEM_CATEGORIA_A = "Entregador não possui categoria A";
        public const string LOCACAO_ENTREGADOR_SEM_PERMISSAO = "Locação não pertence ao entregador";
        public const string ENTREGADOR_SEM_NOTIFICACAO = "Entregador não recebeu notificação";
        public const string PEDIDO_NAO_DISPONIVEL = "Pedido não está com status Disponivel";
        public const string PEDIDO_ENTREGADOR_INCORRETO = "Pedido não pertence ao entregador";
        public const string PEDIDO_NAO_ACEITO_ENTREGUE = "Pedido ainda não foi aceito ou já foi entregue";
    }

}
