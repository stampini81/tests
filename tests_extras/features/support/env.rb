require 'yaml'
CONFIG = YAML.safe_load(
  File.read(File.expand_path('../config/test_config.yaml', __dir__)),
  aliases: true
)[ENV['TEST_ENV'] || 'default']

require 'selenium-webdriver'
require_relative '../pages/pessoa_page'
require_relative '../pages/categoria_page'
require_relative '../pages/transacao_page'


Before do
  chrome_options = Selenium::WebDriver::Chrome::Options.new
  chrome_options.add_argument('--log-level=3') # Fatal only
  chrome_options.add_argument('--disable-logging')
  chrome_options.add_argument('--disable-gpu')
  chrome_options.add_argument('--disable-software-rasterizer')
  chrome_options.add_argument('--disable-dev-shm-usage')
  chrome_options.add_argument('--no-sandbox')
  @driver = Selenium::WebDriver.for :chrome, options: chrome_options
  @driver.manage.timeouts.implicit_wait = CONFIG['timeout']
  @pessoa_page = PessoaPage.new(@driver)
  @categoria_page = CategoriaPage.new(@driver)
  @transacao_page = TransacaoPage.new(@driver)
end

After do
  @driver.quit if @driver
end
