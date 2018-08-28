Pragmatismo.io Methodology

Copyright (c) Pragmatismo.io. All rights reserved.                          
Licensed under the MIT license                                              

6.4 Bots
--------

| Título                                                                     | Endereço                                                                                                                      |
|----------------------------------------------------------------------------|-------------------------------------------------------------------------------------------------------------------------------|
| Alphabetical list of part-of-speech tags used in the Penn Treebank Project | https://www.ling.upenn.edu/courses/Fall_2003/ling001/penn_treebank_pos.html                                                   |
| Transferência de chamadas no Skype For Business                            | https://github.com/pgurenko/Locations                                                                                         |
| Skype for Business URA / IVR                                               | https://github.com/tomorgan/UCMA-IVR-Demo                                                                                     |
| Regras de Gravação Telefônica                                              | http://en.wikipedia.org/wiki/Telephone_recording_laws                                                                         |
| Twitter Dan Driscoll                                                       | https://twitter.com/thedandriscoll/                                                                                           |
| Bot Metrics                                                                | https://github.com/botmetrics/botmetrics                                                                                      |
| Build a real-time media bot for Skype                                      | https://docs.microsoft.com/en-us/bot-framework/dotnet/bot-builder-dotnet-real-time-audio-video-call-overview                  |
| Blog sobre Skype Call e BOT Framework                                      | https://ankitbko.github.io/2016/11/skype-call-your-bot/                                                                       |
| Azure Search para QnA                                                      | https://www.microsoft.com/reallifecode/2016/12/10/leveraging-azure-search-for-implementing-a-qna-bot-in-unsupported-languages |
| Padrão de documentação                                                     | https://github.com/MicrosoftDX/botFramework-botSpecDocs                                                                       |
| Bot Framework Extensions                                                   | https://blogs.msdn.microsoft.com/brandonh/2017/05/18/bot-builder-extensions-extensions-for-microsofts-bot-framework           |
| Microsoft Bot Framework Resources                                          | https://blogs.msdn.microsoft.com/smich/2016/09/30/microsoft-bot-framework-resources/                                          |
| Referência de Ids do BotFramework                                          | https://docs.microsoft.com/en-us/bot-framework/resources-identifiers-guide                                                    |
| Transferring chat to a human agent using Microsoft Bot Framework           | https://ankitbko.github.io/2017/03/human-handover-bot/                                                                        |
| Whatsapp #1 																 | https://chat-api.com/en/demo.html |
| Pagamentos  																 | https://docs.microsoft.com/en-us/azure/bot-service/nodejs/bot-builder-nodejs-request-payment?view=azure-bot-service-3.0 |
| A collection of useful extensions for Bot Builder v4 | https://github.com/Stevenic/botbuilder-toybox |
| Microsoft Cognitive Services Labs                                          | https://labs.cognitive.microsoft.com/ | 
| Alexa Skills															     | https://www.amazon.com/alexa-skills/b/ref=sd_allcat_ods_ha_con_skills_st?ie=UTF8&node=13727921011
| Connecting BOT Framework to Cortana| https://help.knowledge.store/tutorials_code_samples/bot_framework/index.html
| Ligar Cortana | https://help.knowledge.store/tutorials_code_samples/bot_framework/index.html
| Transferir para Humanos | https://github.com/ankitbko/human-handoff-bot
| BOT Framework Offline | https://github.com/ryanvolum/offline_dl


6.4.1 General Bots Plataforma
-----------------------------
https://<SITENAME>.scm.azurewebsites.net/api/vfs/LogFiles/Application/index.html
### 6.4.1.1. Criação de ambiente

1.  Criar uma conta no https://aka.ms/msaappid e salvar o App Id e App Secret no
    .env, seção: Configurações do BOT

2.  Criar uma conta no http://dev.botframework.com

3.  Copiar chave do Web Chat e atualizar a aplicação Web (ChatPane.js)

4.  Criar uma conta no http://luis.ai (NLP) e atualizar as configurações no
    .env, seção: Configurações do LUIS.

### 6.4.1.2. Configurar o BOT Framework Channel Emulator

1.  Abra o BOT Framework Channel Emulator

2.  Configure a URL para http://localhost:4242

3.  Configure o App ID e Password com os valores do .env

### 6.4.1.3. Criar cabeçalho do cliente nos fontes em ASCII

1.  Visite http://patorjk.com/software/taag/\#p=display&h=1&f=Graffiti&t=

2.  Defina os cabeçalhos dos arquivos fontes.

### 6.4.1.4. Como formatar JSON para melhor visualização

Google: Json formater

### 6.4.1.5. Configuração do Web.config

1.  O BotId no web.config recebe o valor do bot handle definido no Portal.

### 6.4.1.6. Mantendo estado dentro de Dialogs

1.  context.PrivateConversationData.SetValue

2.  context.PrivateConversationData.TryGetValue 

3.  Referência:
    http://www.robinosborne.co.uk/2016/08/08/persisting-data-within-a-conversation-with-botframeworks-dialogs/

### 6.4.1.7. Configurando o Bot Framework Channel Emulator no Visual Studio


a. Visual Studio | Tools | External Tools | Add
b. Title: &Bot Framework Channel Emulator
c. Command Line: %APPDATA%\..\Local\botframework\Update.exe
d. Arguments:  --processStart "botframework-emulator.exe"


### 6.4.1.8. Utilizando comandos especiais do Bot Framework Channel Emulator


a. deleteUserData


### 6.4.1.9. Verificando validade da conta do BOT via curl


a. curl -k -X POST https://login.microsoftonline.com/botframework.com/oauth2/v2.0/token -d "grant_type=client_credentials&client_id=7b70604a-8c1e-4757-9153-92c074d51d77&client_secret=XvrrCaSFHsWYrWeVaZ5gKcV&scope=https%3A%2F%2Fapi.botframework.com%2F.default"


### 6.4.1.10. Executando o ngrok para expor o localhost para o BotFramework na Internet


a. ngrok http 4242 -host-header="localhost"


### 6.4.1.11. Realizando dump do conteúdo dos controladores em caso de erro


a. Instalar plugin App do Chrome Post Man
b. Utilizar o Post Man para navegar para http://localhost:2525/api/messages /api/calling/call e verificar o retorno do POST, geralmente um Stack Trace e informações de pacotes NuGet  faltando.


### 6.4.1.12. Atualizando o projeto para Skype API


a. Atualizar callbackurl para Skype 1:1 https://c1e6494c.ngrok.io/api/calling/call
b. Atualizar portal BotFramework
     i. Skype Edit:
    1) 1:1 audio calls
    2) Calling Webhook: https://HOSTNAME/api/calling/call
    c. Atualizar web.config com ngrok;


### 6.4.1.13. Criando Intenções no LUIS

Ao criar uma intenção, utilize sempre um verbo no infinitivo seguido das outras
palavras que descrevem a Ação da pessoa. Ex.: ReiniciarSenha.

### 6.4.1.14. Excelentes Demos em Node.JS

https://github.com/Microsoft/BotBuilder-Samples/tree/master/Node/demo-ContosoFlowers


## 6.5 Procedimentos

### 6.5.1 Adicionando .gbapp, .gbkb, etc dentro de um .gbai como submodule Git

git submodule add https://github.com/pragmatismo-io/ProjectBot.gbkb
git submodule update --init --recursive



## 6.6 Microsoft Bot Framework Links

https://www.npmjs.com/package/botbuilder-topical
https://github.com/Stevenic/botbuilder-toybox
https://github.com/ritazh/botframework-hipchatchannel
Handoff: https://github.com/palindromed/Bot-HandOff
https://www.npmjs.com/package/botbuilder-topical-lite 
https://www.npmjs.com/package/botbuilder-botbldr
https://www.npmjs.com/package/micro-botbuilder 
https://www.npmjs.com/package/botbuilder-unit
https://www.npmjs.com/package/botbuilder-notifications 
https://www.npmjs.com/package/bot-handoff 
https://www.npmjs.com/package/botfarm