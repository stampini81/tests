  def preencher_descricao(descricao)
    campo = @wait.until { @driver.find_element(:css, "input#descricao[name='descricao']") }
    campo.clear
    campo.send_keys(descricao)
  end

  def preencher_data(data)
    campo = @wait.until { @driver.find_element(:css, "input#data[name='data']") }
    campo.click # Garante que o popup de data seja aberto
    campo.clear
    campo.send_keys(data)
  end
# features/pages/transacao_page.rb

class TransacaoPage
  def initialize(driver)
    @driver = driver
    @wait = Selenium::WebDriver::Wait.new(timeout: 10)
  end

  def abrir_modal
    botao = @wait.until { @driver.find_element(:xpath, "//button[contains(., 'Adicionar Transação')]") }
    botao.click
  end

  # Remove duplicado: sempre use o método robusto igual ao de categoria
  def preencher_pessoa(nome)
    campo = @wait.until { @driver.find_element(:css, "input#pessoa-select[aria-label='Lista de pessoas']") }
    campo.clear
    campo.click
    campo.send_keys(nome)
    # Aguarda a lista de opções aparecer
    @wait.until { @driver.find_elements(:css, "[id^='pessoa-select-options'] li, [role='option']").any? }
    # Seleciona a opção correta
    opcoes = @driver.find_elements(:css, "[id^='pessoa-select-options'] li, [role='option']")
    opcao = opcoes.find { |o| o.text.strip.downcase.include?(nome.strip.downcase) }
    opcao&.click
  end

  def preencher_categoria(nome)
    campo = @wait.until { @driver.find_element(:css, "input#categoria-select[aria-label='Lista de categorias']") }
    campo.clear
    campo.click
    campo.send_keys(nome)
    # Aguarda a lista de opções aparecer
    @wait.until { @driver.find_elements(:css, "[id^='categoria-select-options'] li, [role='option']").any? }
    # Seleciona a opção correta
    opcoes = @driver.find_elements(:css, "[id^='categoria-select-options'] li, [role='option']")
    opcao = opcoes.find { |o| o.text.strip.downcase.include?(nome.strip.downcase) }
    opcao&.click
  end

  # (duplicado removido)

  def preencher_valor(valor)
    campo = @wait.until { @driver.find_element(:css, "input#valor[name='valor']") }
    campo.clear
    campo.send_keys(valor)
  end

  def selecionar_tipo(tipo)
    campo = @wait.until { @driver.find_element(:css, "select#tipo[name='tipo']") }
    campo.send_keys(tipo)
  end

  # (duplicado removido)

  def salvar
    # Aguarda overlay sumir antes de buscar o botão
    wait_overlay_sumir
    dialog = @wait.until { @driver.find_element(:css, 'div[role="dialog"][data-state="open"]') }
    botao = @wait.until do
      el = dialog.find_element(:xpath, ".//button[@type='submit' and (contains(., 'Salvar') or contains(., 'Adicionar'))]")
      el if el.displayed? && el.enabled? && visible_and_opacity_ok?(el)
    end
    begin
      botao.click
    rescue Selenium::WebDriver::Error::ElementNotInteractableError, Selenium::WebDriver::Error::ElementClickInterceptedError
      begin
        @driver.action.move_to(botao).click.perform
      rescue
        if botao.displayed? && botao.enabled?
          @driver.execute_script('arguments[0].click();', botao)
        end
      end
    end
  end

  def preencher_descricao(descricao)
    campo = @wait.until { @driver.find_element(:css, "input#descricao[name='descricao']") }
    campo.clear
    campo.send_keys(descricao)
  end

  def preencher_data(data)
    campo = @wait.until { @driver.find_element(:css, "input#data[name='data']") }
    campo.clear
    data_valida = Date.today.strftime('%d/%m/%Y')
    campo.send_keys(data_valida)
  end

  def mensagem_erro
    @wait.until { @driver.find_element(:css, '.error, .alert-danger, .MuiAlert-message, [role="status"]') }.text
  end

  private
  def overlay_presente?
    overlays = @driver.find_elements(:css, 'div[aria-hidden="true"][data-state="open"], div.bg-black\/80, div[data-aria-hidden="true"], div[aria-hidden="true"], div[data-state="open"]')
    overlays.any? { |o| o.displayed? && (o.size.width > 10 && o.size.height > 10) }
  rescue
    false
  end

  def wait_overlay_sumir(timeout = 5)
    Selenium::WebDriver::Wait.new(timeout: 10).until { !overlay_presente? }
  rescue Selenium::WebDriver::Error::TimeoutError
    # Overlay não sumiu, segue mesmo assim
  end

  def visible_and_opacity_ok?(el)
    style = @driver.execute_script('return {visibility: window.getComputedStyle(arguments[0]).visibility, opacity: window.getComputedStyle(arguments[0]).opacity};', el)
    visibility = style['visibility']
    opacity = style['opacity']
    visibility == 'visible' && opacity.to_f > 0.5
  end

  private
  def overlay_presente?
    overlays = @driver.find_elements(:css, 'div[aria-hidden="true"][data-state="open"]')
    overlays.any? { |o| o.displayed? }
  rescue
    false
  end


end
