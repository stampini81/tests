# features/pages/pessoa_page.rb

class PessoaPage
  def initialize(driver)
    @driver = driver
    @wait = Selenium::WebDriver::Wait.new(timeout: 10)
  end

  def abrir_modal
    botao = @wait.until { @driver.find_element(:xpath, "//button[contains(., 'Adicionar Pessoa')]") }
    botao.click
  end

  def preencher_nome(nome)
    campo = @wait.until { @driver.find_element(:css, "input#nome[name='nome']") }
    campo.clear
    campo.send_keys(nome)
  end

  def preencher_idade(data_nascimento)
    campo = @wait.until { @driver.find_element(:css, "input#dataNascimento[name='dataNascimento']") }
    campo.clear
    # Sempre envia no formato dd/mm/yyyy
    if data_nascimento =~ /^\d{4}-\d{2}-\d{2}$/
      # Converte yyyy-mm-dd para dd/mm/yyyy
      partes = data_nascimento.split('-')
      data_valida = "#{partes[2]}/#{partes[1]}/#{partes[0]}"
    else
      data_valida = data_nascimento
    end
    campo.send_keys(data_valida)
  end

  def salvar
    botao = @wait.until { @driver.find_element(:xpath, "//button[contains(., 'Salvar')]") }
    botao.click
  end

  def mensagem_erro
    # Busca mensagem de erro real exibida pelo sistema
    begin
      wait_long = Selenium::WebDriver::Wait.new(timeout: 15)
      msg = wait_long.until { el = @driver.find_element(:css, '[role="status"][aria-live="polite"]'); el.text unless el.text.strip.empty? }
      msg
    rescue Selenium::WebDriver::Error::TimeoutError
      msg = @wait.until { el = @driver.find_element(:css, '.go3958317564'); el.text unless el.text.strip.empty? }
      msg
    end
  end
end
