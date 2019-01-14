//  *********************************************************************************************************
// Empresa: DSBR - Empresa de Desenvolvimento de Sistemas
// Sistema: Automação Comercial
// Projeto: DSBR.ServicesWeb
// Autores: 
// Valnei Filho e-mail: vbatistas@devsysbrasil.com.br;
// Vagner Marcelo e-mail: vmarcelo@devsysbrasil.com.br
// Data Criação:09/09/2018
// Todos os direitos reservados
//  *********************************************************************************************************

#region

using System;
using System.Collections.Generic;
using AutoMapper;
using DSBR.CrossCutting;
using DSBR.CrossCutting.Exceptions;
using DSBR.Domain.Entities;
using DSBR.Infra.Data.EF.Repositories;
using DSBR.ServicesWeb.ViewModel;

#endregion

namespace DSBR.ServicesWeb.Models
{
    public class PagamentoModel : PagamentoRepository
    {
        public static IEnumerable<VencimentoViewModel> CalcularParcelas(PagamentoViewModel comprarViewModel)
        {
            var parcelas = Convert.ToInt16(comprarViewModel.NumeroParcela);
            var capitalInicial = comprarViewModel.ValorLicenca;
            var dataPagamento = comprarViewModel.DataInicioParcela;
            var diasPrimeiraParcela = Convert.ToInt16(comprarViewModel.QtdDiasPrimeriParcela);
            var intervaloParcelas = Convert.ToInt16(comprarViewModel.IntervaloParcela);

            if (parcelas == 0) throw new Exception("Informe o número de parcelas");
            if (capitalInicial == 0) throw new Exception("Informe um valor da licença");
            if (DateTime.Compare(dataPagamento, DateTime.Now) < 0)
                throw new Exception($"Informe uma data maior que {DateTime.Now.ToShortDateString()}");

            var list = Calculadora.Parcelar(parcelas, capitalInicial, dataPagamento, diasPrimeiraParcela, intervaloParcelas, 0, EnumExt.Juros.Nenhum);
            var list2 = Mapper.Map<IEnumerable<VencimentoViewModel>>(list);
            return list2;
        }

        public void Criar(string nomeOperador, PagamentoViewModel comprarViewModel,
                List<VencimentoViewModel> vencimentos)
        {
            foreach (var item in vencimentos)
            {
                if (ExisteFatura(comprarViewModel.IdLicenca, item.DataPagamento))
                    throw new FalhaValidacaoException(
                            $"Já existe uma fatura no mês de {item.DataPagamento.ToString("MMMM")} associada a licença {comprarViewModel.IdLicenca}");
            }

            foreach (var item in vencimentos)
            {
                Repository.Pagamento.Add(new Pagamento
                {
                    NomeOperador = nomeOperador,
                    IdLicenca = comprarViewModel.IdLicenca,
                    DataVencimento = item.DataPagamento,
                    ValorLicenca = item.Valor,
                    FormaPagamento = comprarViewModel.FormaPagamento
                });
            }
            Repository.SaveChanges();
        }
    }
}