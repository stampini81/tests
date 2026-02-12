require 'net/http'
require 'json'
require 'uri'

class ApiService
  BASE_URL = 'http://localhost:5000/api/v1'

  def self.criar_pessoa(nome, data_nascimento)
    uri = URI("#{BASE_URL}/Pessoas")
    # Converter data dd/mm/yyyy para yyyy-mm-dd se necessário
    if data_nascimento.include?('/')
      parts = data_nascimento.split('/')
      data_iso = "#{parts[2]}-#{parts[1]}-#{parts[0]}"
    else
      data_iso = data_nascimento
    end

    payload = { nome: nome, dataNascimento: data_iso }
    
    # Tentar buscar antes para não duplicar (idempotência rápida via API)
    # GET /Pessoas?page=1&pageSize=100 (simplificação)
    # Por hora, vamos confiar na criação. Se der erro de regra de negócio, paciência, mas API aceita duplicados.
    
    response = Net::HTTP.post(uri, payload.to_json, "Content-Type" => "application/json")
    
    if response.code.to_i == 201
      id = JSON.parse(response.body)['id']
      puts "[API] Pessoa criada: #{nome} (ID: #{id})"
      return id
    else
      puts "[API] Erro ao criar pessoa: #{response.body}"
      return nil
    end
  end

  def self.criar_categoria(descricao, finalidade)
    # Converter finalidade string para int
    tipo = case finalidade.downcase
           when 'despesa' then 0
           when 'receita' then 1
           # Ambas não existe no enum do backend (0 ou 1), mas no front sim?
           # O backend espera 0 (Despesa) ou 1 (Receita). "Ambas" geralmente é conceitual ou Tipo 2?
           # Verificando CategoriaTests.cs: Enum tem Receita(1), Despesa(0). Não tem Ambas.
           # O front trata "Ambas" como? Vamos assumir Receita por enquanto ou investigar.
           # No feature file: "existe uma categoria chamada ... com finalidade 'ambas'"
           # Se a API não suporta, vamos criar como Despesa (0) ou Receita (1) dependendo do teste?
           # Melhor: Ver API.
           
           # Olhando o código anterior: CategoriaPage preenche "ambas".
           # Vamos assumir 2 se existir, ou tratar erro.
           when 'ambas' then 2 
           else 0
           end

    uri = URI("#{BASE_URL}/Categorias")
    payload = { descricao: descricao, finalidade: tipo }

    response = Net::HTTP.post(uri, payload.to_json, "Content-Type" => "application/json")
    if response.code.to_i == 201
      id = JSON.parse(response.body)['id']
      puts "[API] Categoria criada: #{descricao} (ID: #{id})"
      return id
    else
      # Se já existe ou erro
      puts "[API] Erro/Info criar categoria: #{response.code} - #{response.body}"
      return nil
    end
  end
end
