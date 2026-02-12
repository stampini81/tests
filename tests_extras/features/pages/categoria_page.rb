# features/pages/categoria_page.rb

class CategoriaPage
  def initialize(driver)
    @driver = driver
    @wait = Selenium::WebDriver::Wait.new(timeout: 10)
  end

  def abrir_modal
    botao = @wait.until { @driver.find_element(:xpath, "//button[contains(., 'Adicionar Categoria')]") }
    botao.click
  end

  def preencher_nome(nome)
    campo = @wait.until { @driver.find_element(:css, "input#descricao[name='descricao']") }
    campo.clear
    campo.send_keys(nome)
  end

  def preencher_finalidade(finalidade)
    campo = @wait.until { @driver.find_element(:css, "select#finalidade[name='finalidade']") }
    campo.send_keys(finalidade)
  end

  def salvar
    botao = @wait.until { @driver.find_element(:xpath, "//button[contains(., 'Salvar')]") }
    botao.click
  end

  def mensagem_erro
    @wait.until { @driver.find_element(:css, '.error, .alert-danger, .MuiAlert-message') }.text
  end
end
