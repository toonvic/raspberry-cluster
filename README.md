# Clusterização de Raspberry utilizando K3S

## Introdução

Este repositório tem como objetivo fornecer um guia passo a passo para a clusterização de dispositivos Raspberry Pi utilizando K3S, uma distribuição leve do Kubernetes. A clusterização de Raspberry Pi é uma abordagem poderosa para distribuir e escalar aplicações em ambientes de computação de borda. Neste exemplo, o foco está em subir uma aplicação que pode ser distribuída entre os nós de processamento, aproveitando as vantagens de escalabilidade e poder computacional oferecidos por uma configuração de cluster.

## Objetivos

Os principais objetivos deste projeto incluem:

1. **Distribuição de Aplicações:** Utilizar o K3S para distribuir e gerenciar aplicações de maneira eficiente entre os nós do cluster.

2. **Aumento de Poder Computacional:** Aproveitar a capacidade de processamento combinada de vários Raspberry Pi para aumentar o poder computacional disponível para as aplicações.

3. **Escalabilidade:** Demonstrar como a adição de novos nós ao cluster pode facilmente escalar a capacidade de processamento disponível para as aplicações.

## Vantagens da Utilização de Raspberry Pi

A escolha do Raspberry Pi para este projeto é baseada em várias vantagens:

1. **Tamanho Compacto:** Os Raspberry Pi são dispositivos compactos, facilitando a implementação em diferentes ambientes.

2. **Processamento Ideal para Projetos Pessoais:** O Raspberry Pi oferece uma combinação ideal de desempenho e consumo de energia, tornando-o adequado para uma variedade de projetos pessoais e experimentações.

3. **Custo Acessível:** A acessibilidade do Raspberry Pi permite a construção de clusters de baixo custo, tornando a clusterização uma opção viável para entusiastas e projetos com recursos limitados.

## Requisitos

Certifique-se de que sua máquina atenda aos seguintes requisitos antes de iniciar o processo de clusterização:

1. **SSH Habilitado:** Certifique-se de ter o SSH habilitado na sua máquina para estabelecer a conexão com os Raspberry Pi.

2. **WSL (Windows Subsystem for Linux):** Caso esteja utilizando o Windows, certifique-se de ter o WSL instalado e configurado corretamente para facilitar a interação com ambientes baseados em Linux.

3. **Virtualização Habilitada:** Verifique se a virtualização está ativada na BIOS/UEFI da sua máquina para garantir o funcionamento adequado de ambientes virtualizados necessários para a configuração do cluster.

4. **Pelo Menos 2 Raspberry Pi:** É necessário ter pelo menos dois dispositivos Raspberry Pi para formar o cluster. Eles servirão como nós de processamento no seu ambiente distribuído.

5. **Conexão com a Internet:** Certifique-se de que os Raspberry Pi tenham acesso à internet, seja por meio de cabos Ethernet ou conexão Wi-Fi.

6. **Disco para Cada Raspberry Pi:** Cada Raspberry Pi deve ter um meio de armazenamento, como um cartão SD, microSD, SSD ou unidade USB, para o sistema operacional e aplicativos.

7. **Raspberry Pi Imager:** Tenha o Raspberry Pi Imager instalado em sua máquina para facilitar o processo de instalação do sistema operacional nos Raspberry Pi.

### Instalação do OS via Raspberry Pi Imager:

Para instalar o sistema operacional nos Raspberry Pi, utilizaremos o Raspberry Pi Imager, uma ferramenta intuitiva que simplifica o processo. Siga estes passos:

1. **Download e Instalação do Raspberry Pi Imager:**
   - Faça o download do Raspberry Pi Imager no [site oficial](https://www.raspberrypi.org/software/) e siga as instruções de instalação para o seu sistema operacional.

2. **Execução do Raspberry Pi Imager:**
   - Abra o Raspberry Pi Imager após a instalação.

3. **Seleção do Sistema Operacional:**
   - No Raspberry Pi Imager, clique em "Choose OS" e selecione a distribuição de sistema operacional desejada. Por exemplo, escolha o "Raspberry Pi OS (32-bit)".

4. **Seleção do Cartão SD ou Dispositivo de Armazenamento:**
   - Clique em "Choose SD Card" e selecione o cartão SD, microSD, SSD ou unidade USB que será usado para instalar o sistema operacional.

5. **Configuração Headless (Sem Monitor):**
   - Para uma configuração sem monitor (headless), durante a gravação, clique em "Ctrl + Shift + X" ou selecione "Edit" para acessar as configurações avançadas.
   
6. **Preenchimento de Informações:**
   - Preencha as informações do seu usuário, incluindo nome, senha desejada e o nome do seu Raspberry (hostname).

7. **Configuração da Rede Wi-Fi (Opcional):**
   - Caso esteja utilizando Wi-Fi, especifique as informações de rede, como o nome (SSID) e a senha.

8. **Habilitação do SSH via Autenticação de Senha:**
   - Certifique-se de habilitar o SSH para permitir a conexão remota. Isso pode ser feito nas configurações avançadas.

9. **Gravação do Sistema Operacional:**
   - Após configurar todas as opções, clique em "Write" para iniciar o processo de gravação do sistema operacional no dispositivo.

10. **Conclusão e Remoção do Dispositivo:**
    - Assim que a gravação estiver concluída, o Raspberry Pi Imager exibirá uma mensagem de sucesso. Remova com segurança o cartão SD ou o dispositivo de armazenamento do seu computador.

![Raspberry Pi Imager](caminho/para/imagem.jpg)

Agora, o sistema operacional está pronto para ser utilizado nos Raspberry Pi. Este procedimento deve ser repetido para cada Raspberry Pi no cluster.
