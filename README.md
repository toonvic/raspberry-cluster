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

Certifique-se de atender aos seguintes requisitos:

1. **SSH Habilitado:** Certifique-se de ter o SSH habilitado na sua máquina para estabelecer a conexão com os Raspberry Pi.

2. **WSL (Windows Subsystem for Linux):** Caso esteja utilizando o Windows, certifique-se de ter o WSL instalado e configurado corretamente para facilitar a interação com ambientes baseados em Linux.

3. **Virtualização Habilitada:** Verifique se a virtualização está ativada na BIOS/UEFI da sua máquina para garantir o funcionamento adequado de ambientes virtualizados necessários para a configuração do cluster.

4. **2+ Raspberry Pi:** É necessário ter pelo menos dois dispositivos Raspberry Pi para formar o cluster. Eles servirão como nós de processamento no seu ambiente distribuído.

5. **Conexão com a Internet:** Certifique-se de que os Raspberry Pi tenham acesso à internet, seja por meio de cabos Ethernet ou conexão Wi-Fi.

6. **Disco para Cada Raspberry Pi:** Cada Raspberry Pi deve ter um meio de armazenamento, como um cartão SD, microSD, SSD ou unidade USB, para o sistema operacional e aplicativos.

7. **Raspberry Pi Imager:** Tenha o [Raspberry Pi Imager](https://www.raspberrypi.org/software/) instalado em sua máquina para realizar a instalação headless dos SOs em seus Raspberry.

### Instalação do OS via Raspberry Pi Imager:

Para instalar o sistema operacional nos Raspberry Pi, utilizaremos o Raspberry Pi Imager, uma ferramenta intuitiva que simplifica o processo. Siga estes passos:

1. **Download e Instalação do Raspberry Pi Imager:**
   - Faça o download do Raspberry Pi Imager no [site oficial](https://www.raspberrypi.org/software/) e siga as instruções de instalação para o seu sistema operacional.
  
2. **Seleção do Dispositivo:**
   - Clique em "Choose Device" e selecione o modelo do seu dispositivo Raspberry.

3. **Seleção do Sistema Operacional:**
   - Clique em "Choose OS" e selecione a distribuição de sistema operacional desejada. Recomendo utilizar a versão do Raspberry OS Legacy 64-bit Lite.

4. **Seleção do Cartão SD ou Dispositivo de Armazenamento:**
   - Clique em "Choose Storage" e selecione a unidade de disco que será utilizada para instalar o sistema operacional.

5. **Configuração Headless (Sem Monitor):**
   - Clique "Ctrl + Shift + X" ou selecione "Edit" após "Next" para acessar as configurações avançadas.
   
6. **Preenchimento de Informações:**
   - Preencha as informações do seu usuário, incluindo nome, senha desejada e o nome do seu Raspberry (hostname).

7. **Configuração da Rede Wi-Fi (Opcional):**
   - Caso esteja utilizando Wi-Fi, especifique as informações de rede, como o nome (SSID) e a senha.

8. **Habilitação do SSH via Autenticação de Senha:**
   - Na aba "Services", certifique-se de habilitar o SSH para permitir a conexão remota.

9. **Gravação do Sistema Operacional:**
   - Clique em "Next" e confirme a limpeza do disco rígido para iniciar o processo de gravação do sistema operacional no dispositivo.

10. **Conclusão e Remoção do Dispositivo:**
    - Assim que a gravação estiver concluída, o Raspberry Pi Imager exibirá uma mensagem de sucesso.

![Raspberry Pi Imager](caminho/para/imagem.jpg)

Agora, o sistema operacional está pronto para ser utilizado nos Raspberry Pi. Este procedimento deve ser repetido para cada Raspberry Pi no cluster.

### Instalação do K3S:

O K3S é um Kubernetes leve e fácil de usar, perfeito para ambientes distribuídos como o Raspberry Pi. Siga os passos abaixo para instalar o K3S nos seus dispositivos:

1. **Preparação do Raspberry Pi:**
   - Antes de instalar o K3S, é necessário ajustar a configuração do sistema. Execute o seguinte comando para editar o arquivo `cmdline.txt`:
     ```bash
     sudo nano /boot/cmdline.txt
     ```
   - Adicione as variáveis `cgroup_memory=1 cgroup_enable=memory` ao final do conteúdo do arquivo. O trecho de código modificado deve se parecer com isso:

     ```plaintext
     console=serial0,115200 console=tty1 root=PARTUUID=b638588e-02 rootfstype=ext4 fsck.repair=yes rootwait cgroup_memory=1 cgroup_enable=memory
     ```

   - Após editar o arquivo, salve as alterações e saia do editor.
  
**Configuração do IP Fixo:**

Após instalar o K3S, é recomendável configurar um IP fixo para garantir consistência na comunicação do cluster. Siga os passos abaixo:

1. **Obtenção do Endereço IP e Gateway:**
   - Execute o seguinte comando para obter o endereço IP do Raspberry Pi:
     ```bash
     hostname -I
     ```
   - Anote o endereço IP obtido e execute o seguinte comando para encontrar o gateway padrão:
     ```bash
     ip r
     ```
   - Anote o endereço do gateway.

2. **Configuração do IP Estático:**
   - Abra o arquivo `dhcpcd.conf` para edição:
     ```bash
     sudo nano /etc/dhcpcd.conf
     ```
   - Localize a seção relacionada às configurações de IP estático, que normalmente está comentada. O trecho de código deve se parecer com isso:
     ```plaintext
     # Example static IP configuration:
     #interface eth0
     #static ip_address=192.168.1.10/24
     #static ip6_address=fd51:42f8:caae:d92e::ff/64
     #static routers=192.168.1.1
     #static domain_name_servers=192.168.1.1 8.8.8.8 fd51:42f8:caae:d92e::1
     ```
   - Descomente as linhas removendo o `#` e ajuste as configurações conforme o IP e gateway anotados anteriormente. Por exemplo:
     ```plaintext
     interface eth0
     static ip_address=SEU_ENDERECO_IP/24
     static routers=SEU_GATEWAY
     static domain_name_servers=SEU_GATEWAY 8.8.8.8
     ```
   - Salve as alterações e saia do editor.

3. **Reinicie o Raspberry Pi:**
   - Reinicie o Raspberry Pi para aplicar as configurações:
     ```bash
     sudo reboot
     ```

Agora, o Raspberry Pi está configurado com um IP fixo, proporcionando estabilidade na comunicação do cluster K3S.
