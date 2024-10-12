using MinimalApi.Dominio.Entidade;
using MinimalApi.Dominio.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Test.Mocks
{
    public class VeiculoServicoMock : IVeiculoServico
    {
        private List<Veiculo> veiculos = new List<Veiculo>
        {
            new Veiculo { Id = 1, Nome = "Corolla", Marca = "Toyota", Ano = 2020 },
            new Veiculo { Id = 2, Nome = "Civic", Marca = "Honda", Ano = 2019 }
        };

        public void Apagar(Veiculo veiculo)
        {
            var veiculoExistente = BuscaPorId(veiculo.Id);
            if (veiculoExistente != null)
            {
                veiculos.Remove(veiculoExistente); // Remove a instância existente
            }
        }

        public void Atualizar(Veiculo veiculo)
        {
            var existingVeiculo = veiculos.FirstOrDefault(v => v.Id == veiculo.Id);
            if (existingVeiculo != null)
            {
                existingVeiculo.Nome = veiculo.Nome;
                existingVeiculo.Marca = veiculo.Marca;
                existingVeiculo.Ano = veiculo.Ano;
            }
        }

        public Veiculo? BuscaPorId(int id)
        {
            return veiculos.FirstOrDefault(v => v.Id == id);
        }

        public void Incluir(Veiculo veiculo)
        {
            veiculo.Id = veiculos.Count + 1; // Simula um ID gerado automaticamente
            veiculos.Add(veiculo);
        }

        public List<Veiculo> Todos(int? pagina = 1, string? nome = null, string? marca = null)
        {
            var query = veiculos.AsQueryable();

            if (!string.IsNullOrEmpty(nome))
            {
                query = query.Where(v => v.Nome.ToLower().Contains(nome.ToLower()));
            }

            if (!string.IsNullOrEmpty(marca))
            {
                query = query.Where(v => v.Marca.ToLower().Contains(marca.ToLower()));
            }

            int itensPorPagina = 10;

            if (pagina != null)
            {
                query = query.Skip(((int)pagina - 1) * itensPorPagina).Take(itensPorPagina);
            }

            return query.ToList();
        }
    }
}
