# Step para: When tento cadastrar uma despesa para a pessoa maior {string} usando a categoria {string}
When('tento cadastrar uma despesa para a pessoa maior {string} usando a categoria {string}') do |nome, categoria|
  @driver.navigate.to "#{CONFIG['base_url']}/transacoes"
  @transacao_page.abrir_modal
  @transacao_page.selecionar_tipo('Despesa')
  @transacao_page.preencher_descricao('Despesa Teste')
  @transacao_page.preencher_valor('50')
  @transacao_page.preencher_data(Date.today.strftime('%d/%m/%Y'))
  @transacao_page.preencher_pessoa(nome)
  @transacao_page.preencher_categoria(categoria)
  @transacao_page.salvar
end

# Step para: When tento cadastrar uma receita para a pessoa maior {string} usando a categoria {string}
When('tento cadastrar uma receita para a pessoa maior {string} usando a categoria {string}') do |nome, categoria|
  @driver.navigate.to "#{CONFIG['base_url']}/transacoes"
  @transacao_page.abrir_modal
  @transacao_page.selecionar_tipo('Receita')
  @transacao_page.preencher_descricao('Receita Teste')
  @transacao_page.preencher_valor('100')
  @transacao_page.preencher_data(Date.today.strftime('%d/%m/%Y'))
  @transacao_page.preencher_pessoa(nome)
  @transacao_page.preencher_categoria(categoria)
  @transacao_page.salvar
end
# Step para: Given existe uma pessoa maior chamada {string} com data de nascimento {string}
Given('existe uma pessoa maior chamada {string} com data de nascimento {string}') do |nome, data|
  @driver.navigate.to "#{CONFIG['base_url']}/pessoas"
  @pessoa_page.abrir_modal
  @pessoa_page.preencher_nome(nome)
  @pessoa_page.preencher_idade(data)
  @pessoa_page.salvar
  wait = Selenium::WebDriver::Wait.new(timeout: 8)
  begin
    wait.until { @driver.find_element(:xpath, "//button[contains(., 'Adicionar Pessoa')]").displayed? }
  rescue Selenium::WebDriver::Error::TimeoutError
  end
end

# Step para: When tento cadastrar uma despesa para a pessoa menor {string}
When('tento cadastrar uma despesa para a pessoa menor {string}') do |nome|
  @contexto = { tipo: 'Despesa', pessoa: nome, categoria: 'CategoriaDespesa' }
  @driver.navigate.to "#{CONFIG['base_url']}/transacoes"
  @transacao_page.abrir_modal
  @transacao_page.selecionar_tipo('Despesa')
  @transacao_page.preencher_descricao('Despesa Teste')
  @transacao_page.preencher_valor('50')
  @transacao_page.preencher_data(Date.today.strftime('%d/%m/%Y'))
  @transacao_page.preencher_pessoa(nome)
  @transacao_page.preencher_categoria('CategoriaDespesa')
  @transacao_page.salvar
end

# Step para: Given existe uma categoria chamada {string} com finalidade {string}
Given('existe uma categoria chamada {string} com finalidade {string}') do |nome, finalidade|
  @driver.navigate.to "#{CONFIG['base_url']}/categorias"
  @categoria_page.abrir_modal
  @categoria_page.preencher_nome(nome)
  @categoria_page.preencher_finalidade(finalidade)
  @categoria_page.salvar
end

# Step para: When tento cadastrar uma despesa para a pessoa maior {string}
When('tento cadastrar uma despesa para a pessoa maior {string}') do |nome|
  @driver.navigate.to "#{CONFIG['base_url']}/transacoes"
  @transacao_page.abrir_modal
  @transacao_page.selecionar_tipo('Despesa')
  @transacao_page.preencher_descricao('Despesa Teste')
  @transacao_page.preencher_valor('50')
  @transacao_page.preencher_data(Date.today.strftime('%d/%m/%Y'))
  @transacao_page.preencher_pessoa(nome)
  @transacao_page.preencher_categoria('Categoria Teste')
  @transacao_page.salvar
end

# Step para: When tento cadastrar uma receita para a pessoa maior {string}
When('tento cadastrar uma receita para a pessoa maior {string}') do |nome|
  @driver.navigate.to "#{CONFIG['base_url']}/transacoes"
  @transacao_page.abrir_modal
  @transacao_page.selecionar_tipo('Receita')
  @transacao_page.preencher_descricao('Receita Teste')
  @transacao_page.preencher_valor('100')
  @transacao_page.preencher_data(Date.today.strftime('%d/%m/%Y'))
  @transacao_page.preencher_pessoa(nome)
  @transacao_page.preencher_categoria('Categoria Teste')
  @transacao_page.salvar
end

# Step para: When tento cadastrar uma despesa para a pessoa menor {string} usando a categoria {string}
When('tento cadastrar uma despesa para a pessoa menor {string} usando a categoria {string}') do |nome, categoria|
  
  @contexto = { tipo: 'Despesa', pessoa: nome, categoria: categoria }
  
  @driver.navigate.to "#{CONFIG['base_url']}/transacoes"
  @transacao_page.abrir_modal
  @transacao_page.selecionar_tipo('Despesa')
  @transacao_page.preencher_descricao('Despesa Teste')
  @transacao_page.preencher_valor('50')
  @transacao_page.preencher_data(Date.today.strftime('%d/%m/%Y'))
  @transacao_page.preencher_pessoa(nome)
  @transacao_page.preencher_categoria(categoria)
  @transacao_page.salvar
end

# Step para: When tento cadastrar uma receita para a pessoa menor {string} usando a categoria {string}
When('tento cadastrar uma receita para a pessoa menor {string} usando a categoria {string}') do |nome, categoria|
  @driver.navigate.to "#{CONFIG['base_url']}/transacoes"
  @transacao_page.abrir_modal
  @transacao_page.selecionar_tipo('Receita')
  @transacao_page.preencher_descricao('Receita Teste')
  @transacao_page.preencher_valor('100')
  @transacao_page.preencher_data(Date.today.strftime('%d/%m/%Y'))
  @transacao_page.preencher_pessoa(nome)
  @transacao_page.preencher_categoria(categoria)
  @transacao_page.salvar
end

# Step para: Then o sistema deve permitir o cadastro

# Step para: Then o sistema deve permitir o cadastro (com validação de regra de negócio)

# Step para: Then o sistema deve permitir o cadastro (com validação de regra de negócio)

# Step para: Then o sistema deve permitir o cadastro (com lógica de negócio para maior/menor)

Then('o sistema deve permitir o cadastro') do
  wait = Selenium::WebDriver::Wait.new(timeout: 10)

  # Busca por erros de forma abrangente, incluindo role="status" e texto "não podem"
  erros = @driver.find_elements(:xpath, "//*[@role='status' or contains(@class, 'error') or contains(@class, 'MuiAlert-message-error') or contains(text(), 'Erro') or contains(text(), 'não podem')]")

  erro_visivel = erros.find { |e| e.displayed? && !e.text.strip.empty? }
  if erro_visivel
    raise "O cadastro deveria ser permitido, mas o sistema bloqueou com a mensagem: '#{erro_visivel.text}'"
  end

  # 2. Aguardamos a mensagem de sucesso aparecer
  begin
    mensagem = wait.until do
      el = @driver.find_element(:xpath, "//*[contains(translate(text(), 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'), 'sucesso')]")
      el if el.displayed?
    end
    expect(mensagem).not_to be_nil
  rescue Selenium::WebDriver::Error::TimeoutError
    botao_salvar = @driver.find_elements(:xpath, "//button[contains(., 'Salvar')]")
    expect(botao_salvar.any?(&:displayed?)).to be(false), "O modal não fechou e nenhuma mensagem de sucesso foi detectada."
  end
end

# Step para: Then o sistema deve exibir a mensagem de erro de finalidade


# Step para: Then o sistema deve exibir a mensagem de erro de finalidade (com validação de regra de negócio)

# Step para: Then o sistema deve exibir a mensagem de erro de finalidade (com validação de regra de negócio)


Then('o sistema deve exibir a mensagem de erro de finalidade') do
  wait = Selenium::WebDriver::Wait.new(timeout: 10)
  begin
    mensagem_erro = wait.until do
      el = @driver.find_element(:xpath, "//*[@role='status' or contains(text(), 'Erro ao salvar') or contains(text(), 'Menores de 18 anos não podem registrar receitas.') or contains(text(), 'não pode') or contains(text(), 'finalidade')]")
      el if el.displayed? && !el.text.strip.empty?
    end
    puts "[DEBUG] Mensagem de erro capturada: #{mensagem_erro.text}"
    expect(mensagem_erro.text).to match(/Erro ao salvar|Menores de 18 anos não podem registrar receitas|não pode|finalidade/i)
  rescue Selenium::WebDriver::Error::TimeoutError
    @driver.save_screenshot('falha_inesperada.png')
    raise "Esperava uma mensagem de erro, mas o modal continua aberto sem avisos ou foi fechado sem sucesso."
  end
end
require 'rspec/expectations'
# Step para: Given existe uma pessoa cadastrada com idade menor que {int} anos
Given('existe uma pessoa cadastrada com idade menor que {int} anos') do |idade|
  @driver.navigate.to "#{CONFIG['base_url']}/pessoas"
  @pessoa_page.abrir_modal
  @pessoa_page.preencher_nome('123456')
  if @pessoa_page.respond_to?(:preencher_idade)
    # Garante que o ano seja maior ou igual a 2010 para pessoa menor
    hoje = Date.today
    ano = [hoje.year - idade.to_i, 2010].max
    data_nascimento = Date.new(ano, hoje.month, hoje.day).strftime('%Y-%m-%d')
    @pessoa_page.preencher_idade(data_nascimento)
  else
    campo = @driver.find_element(:css, "input[name*='data'], input[placeholder*='Data']")
    ano = [Date.today.year - idade.to_i, 2010].max
    data_nascimento = Date.new(ano, Date.today.month, Date.today.day).strftime('%Y-%m-%d')
    campo.send_keys(data_nascimento)
  end
  @pessoa_page.salvar
  # Aguarda o modal fechar (botão de adicionar pessoa deve voltar a aparecer)
  wait = Selenium::WebDriver::Wait.new(timeout: 8)
  begin
    wait.until { @driver.find_element(:xpath, "//button[contains(., 'Adicionar Pessoa')]").displayed? }
  rescue Selenium::WebDriver::Error::TimeoutError
    # Se não fechar, segue o fluxo para não travar o cenário
  end
  # Agora aguarda a pessoa aparecer na lista, mas não bloqueia o fluxo se não encontrar
  begin
    wait.until { @driver.find_elements(:xpath, "//td[contains(., 'Pessoa Menor')]").any? }
  rescue Selenium::WebDriver::Error::TimeoutError
    # Apenas loga, mas segue o fluxo
    puts "Aviso: Pessoa Menor não apareceu na lista imediatamente. Prosseguindo com o teste."
  end
  # Após cadastrar e (tentar) verificar a pessoa, navega para o menu de transações
  begin
    menu_transacoes = @driver.find_element(:xpath, "//a[contains(@href, '/transacoes') or contains(., 'Transações')]")
    menu_transacoes.click
  rescue Selenium::WebDriver::Error::NoSuchElementError
    # Se não encontrar o menu, tenta acessar diretamente
    @driver.navigate.to "#{CONFIG['base_url']}/transacoes"
  end
end

# Step para: Given existe uma categoria cadastrada com finalidade {string}
Given('existe uma categoria cadastrada com finalidade {string}') do |finalidade|
  @driver.navigate.to "#{CONFIG['base_url']}/categorias"
  @categoria_page.abrir_modal
  @categoria_page.preencher_nome('Categoria Teste')
  @categoria_page.preencher_finalidade(finalidade)
  @categoria_page.salvar
end

Given('estou na página de cadastro de pessoas') do
  @driver.navigate.to "#{CONFIG['base_url']}/pessoas"
end

Given('cadastro uma pessoa com idade menor que 18 anos') do
  @pessoa_page.abrir_modal
  @pessoa_page.preencher_nome('Teste Menor')
  # Supondo que o método preencher_idade aceite data de nascimento ou idade
  # Se for data de nascimento, ajuste o método na PessoaPage
  if @pessoa_page.respond_to?(:preencher_idade)
    @pessoa_page.preencher_idade('01/01/2016')
  else
    # fallback para campo data_nasc se necessário
    campo = @driver.find_element(:css, "input[name*='data'], input[placeholder*='Data']")
    campo.send_keys('01/01/2016')
  end
  @pessoa_page.salvar
end

Given('existe uma pessoa menor chamada {string} com data de nascimento {string}') do |nome, data|
  @driver.navigate.to "#{CONFIG['base_url']}/pessoas"
  @pessoa_page.abrir_modal
  @pessoa_page.preencher_nome(nome)
  @pessoa_page.preencher_idade(data)
  @pessoa_page.salvar
  wait = Selenium::WebDriver::Wait.new(timeout: 8)
  begin
    wait.until { @driver.find_element(:xpath, "//button[contains(., 'Adicionar Pessoa')]").displayed? }
  rescue Selenium::WebDriver::Error::TimeoutError
  end
end

# Exemplo: você pode criar um TransacaoPage futuramente para encapsular essa lógica



When('tento cadastrar uma receita para essa pessoa') do
  @driver.navigate.to "#{CONFIG['base_url']}/transacoes"
  @transacao_page.abrir_modal
  @transacao_page.preencher_descricao('Receita Teste')
  @transacao_page.preencher_valor('100')
  # Garante data de hoje no formato yyyy-mm-dd
  data_hoje = Date.today.strftime('%Y-%m-%d')
  @transacao_page.preencher_data(data_hoje)
  @transacao_page.selecionar_tipo('Receita')
  @transacao_page.preencher_pessoa('Pessoa Menor')
  @transacao_page.preencher_categoria('Categoria Teste')
  @transacao_page.salvar
end

When('tento cadastrar uma receita para a pessoa menor {string}') do |nome|
  @driver.navigate.to "#{CONFIG['base_url']}/transacoes"
  @transacao_page.abrir_modal
  @transacao_page.selecionar_tipo('Receita')
  @transacao_page.preencher_descricao('Receita Teste')
  @transacao_page.preencher_valor('100')
  @transacao_page.preencher_data(Date.today.strftime('%d/%m/%Y'))
  @transacao_page.preencher_pessoa(nome)
  @transacao_page.preencher_categoria('Categoria Teste')
  @transacao_page.salvar
end


Then('o sistema deve impedir o cadastro e exibir uma mensagem de erro') do
  # Tenta buscar mensagem de erro genérica, ou use o método da page se aplicável
  if @pessoa_page.respond_to?(:mensagem_erro)
    erro = @pessoa_page.mensagem_erro
    expect(erro.downcase).to match(/menor|não pode|erro/)
  else
    wait = Selenium::WebDriver::Wait.new(timeout: 10)
    erro = wait.until { @driver.find_element(:xpath, "//*[contains(text(), 'menor de idade') or contains(text(), 'não pode') or contains(text(), 'erro')]") }
    expect(erro.text.downcase).to match(/menor|não pode|erro/)
  end
end

Then('o sistema deve exibir a mensagem de bloqueio de receita para menor') do
  erro = @transacao_page.mensagem_erro
  expect(erro.downcase).to include('menores de 18 anos não podem registrar receitas')
end

Given('estou na página de cadastro de categorias') do
  @driver.navigate.to "#{CONFIG['base_url']}/categorias"
end

Given('cadastro uma categoria com finalidade {string}') do |finalidade|
  @categoria_page.abrir_modal
  @categoria_page.preencher_nome('Categoria Teste')
  @categoria_page.preencher_finalidade(finalidade)
  @categoria_page.salvar
end


# Exemplo: crie um TransacaoPage futuramente para encapsular essa lógica



When('tento cadastrar uma receita usando essa categoria') do
  @driver.navigate.to "#{CONFIG['base_url']}/transacoes"
  @transacao_page.abrir_modal
  @transacao_page.preencher_descricao('Receita Teste')
  @transacao_page.preencher_valor('200')
  @transacao_page.preencher_data(Date.today.strftime('%Y-%m-%d'))
  @transacao_page.selecionar_tipo('Receita')
  @transacao_page.preencher_pessoa('Pessoa Menor')
  @transacao_page.preencher_categoria('Categoria Teste')
  @transacao_page.salvar
end


Then('o sistema deve impedir o cadastro e exibir uma mensagem de erro de finalidade') do
  if @categoria_page.respond_to?(:mensagem_erro)
    erro = @categoria_page.mensagem_erro
    expect(erro.downcase).to match(/finalidade|não pode|erro/)
  else
    wait = Selenium::WebDriver::Wait.new(timeout: 10)
    erro = wait.until { @driver.find_element(:xpath, "//*[contains(text(), 'finalidade') or contains(text(), 'não pode') or contains(text(), 'erro')]") }
    expect(erro.text.downcase).to match(/finalidade|não pode|erro/)
  end
end
