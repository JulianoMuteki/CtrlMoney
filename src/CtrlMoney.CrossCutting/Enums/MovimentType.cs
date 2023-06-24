
using System.ComponentModel;

namespace CtrlMoney.CrossCutting.Enums
{
    public enum MovimentType
    {
        None,
        [Description("Amortização")]
        AMORTIZACAO,
        [Description("Leilão de Fração")]
        LEILAO_DE_FRACAO,
        [Description("Recibo de Subscrição")]
        RECIBO_DE_SUBSCRICAO,
        [Description("Bonificação em Ativos")]
        BONIFICACAO_EM_ATIVOS,
        [Description("Fração em Ativos")]
        FRACAO_EM_ATIVOS,
        [Description("Atualização")]
        ATUALIZACAO
    }
}
