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

## Instalação do OS via Raspberry Pi Imager

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
   - Certifique-se de **nomear diferentes hostnames para cada Raspberry (master, node1, node2)**.

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

## Instalação do K3S:

O K3S é um Kubernetes leve e fácil de usar, perfeito para dispositivos como o Raspberry Pi.

**Configurações do dispositivo:**

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

É recomendável configurar um IP fixo para garantir consistência na comunicação do cluster. Siga os passos abaixo:

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

Até este ponto, a configuração é **comum tanto para o nó master quanto para os seus nós**. (●'◡'●)

### Instalação do K3S - Nó Master:

1. **Acesso como Superusuário:**
   - Antes de prosseguir, torne-se superusuário executando:
     ```bash
     sudo su -
     ```
   - Este comando concede privilégios administrativos, facilitando a execução de operações que exigem permissões elevadas.

2. **Instalação do K3S:**
   - Execute o seguinte comando para instalar o K3S no nó master:
     ```bash
     curl -sfL https://get.k3s.io/ | sh -s - --write-kubeconfig-mode 644
     ```
     O parâmetro `--write-kubeconfig-mode 644` define as permissões do arquivo kubeconfig.

3. **Verificação da Instalação:**
   - Após a instalação, verifique se o nó master está em execução. Execute o seguinte comando:
     ```bash
     kubectl get nodes
     ```
Isso deve exibir o nó master como pronto no cluster K3S.

### Instalação do K3S - Nós Trabalhadores:

1. **Obtenção do Token e IP do Nó Master:**
   - Execute os seguintes comandos no nó master para obter as informações necessárias:
     ```bash
     token=$(cat /var/lib/rancher/k3s/server/node-token)
     master_ip=$(hostname -I)
     ```

2. **Instalação do K3S nos Nós Trabalhadores:**
   - Substitua `SEU_TOKEN_AQUI` pelo valor de `$token` e `SEU_IP_MASTER_AQUI` pelo valor de `$master_ip` no comando abaixo:
     ```bash
     curl -sfL https://get.k3s.io/ | K3S_TOKEN="SEU_TOKEN_AQUI" K3S_URL="https://SEU_IP_MASTER_AQUI:6443/" sh -
     ```
     Este comando instala o K3S nos nós trabalhadores, conectando-os ao nó master.

3. **Verificação da Conexão:**
   - Após a instalação, verifique se os nós trabalhadores estão conectados ao cluster. Execute o seguinte comando no nó master:
     ```bash
     kubectl get nodes
     ```
Os nós trabalhadores devem aparecer como "prontos" no cluster K3S.

# Implantação da Greetings API no Kubernetes

A seguir está o arquivo YAML para implantar a Greetings API em um cluster Kubernetes. O código está disponível no [GitHub](https://github.com/seu-usuario/seu-repo/caminho/para/o/arquivo.yaml).

Você pode criar um arquivo utilizando o comando abaixo e colar o conteúdo no mesmo:
```bash
sudo nano nome-do-arquivo.yaml
```

```yaml
# Este arquivo Kubernetes YAML define a configuração para implantar a aplicação Greetings API em um cluster K8s.

# Deployment para a Greetings API
apiVersion: apps/v1
kind: Deployment
metadata:
  name: greetings-api-deployment
  labels:
    app: greetings-api
spec:
  replicas: 6  # Número desejado de réplicas para escalabilidade
  selector:
    matchLabels:
      app: greetings-api
  template:
    metadata:
      labels:
        app: greetings-api
    spec:
      containers:
        - name: greetings-api
          image: victordoamaral/greetingsapi:1.2.3  # Imagem da aplicação com a versão específica
          imagePullPolicy: Always  # Garante que a imagem seja sempre baixada
          ports:
            - containerPort: 80  # Porta na qual a aplicação está escutando

---

# RoleBinding para conceder permissões ao ServiceAccount padrão
apiVersion: rbac.authorization.k8s.io/v1
kind: RoleBinding
metadata:
  name: pod-reader-binding
  namespace: default
subjects:
- kind: ServiceAccount
  name: default
  namespace: default
roleRef:
  kind: Role
  name: pod-reader
  apiGroup: rbac.authorization.k8s.io

---

# Role para definir permissões específicas para leitura de pods
apiVersion: rbac.authorization.k8s.io/v1
kind: Role
metadata:
  namespace: default
  name: pod-reader
rules:
- apiGroups: [""]
  resources: ["pods"]
  verbs: ["get", "list"]

---

# Serviço para expor a Greetings API
apiVersion: v1
kind: Service
metadata:
  name: greetings-api-service
spec:
  selector:
    app: greetings-api
  ports:
    - protocol: TCP
      port: 80
      targetPort: 80
  type: NodePort  # Expõe o serviço externamente via NodePort

---

# Ingress para rotear tráfego para o serviço Greetings API
apiVersion: networking.k8s.io/v1
kind: Ingress
metadata:
  name: greetings-api-ingress
spec:
  rules:
  - host: raspberry.local  # Substitua pelo domínio ou IP de sua escolha
    http:
      paths:
      - path: /greetings
        pathType: Prefix
        backend:
          service:
            name: greetings-api-service
            port:
              number: 80
```


## Deploy da Aplicação e Verificação:

1. **Edição do arquivo hosts:**
   - Edite o arquivo `hosts` (do sistema ao qual você quer utilizar para acessar a rota) para incluir a entrada correspondente ao host configurado no Ingress, apontando para o IP do seu cluster. Exemplo: `192.168.1.7 raspberry.local`.

2. **Aplicação do arquivo YAML:**
   - Aplique o arquivo YAML utilizando o comando:
     ```bash
     kubectl apply -f nome-do-arquivo.yaml
     ```

3. **Verificação dos Pods:**
   - Verifique se os pods estão em execução com o comando:
     ```bash
     kubectl get pods
     ```

4. **Verificação dos Serviços:**
   - Confira se os serviços foram criados corretamente com o comando:
     ```bash
     kubectl get services
     ```

5. **Acesso à Greetings API:**
   - Acesse a Greetings API pelo endereço correspondente, como `http://raspberry.local/greetings`.

Você verá que a API GET disponibilizada na rota `/greetings` irá retornar a listagem de pods relacionados aos seus respectivos nós, além da informação de qual nó está tratando a requisição a partir do balanço de carga.

Dessa forma, podemos compreender os conceitos de escabilidade e balanceamento de carga sendo aplicados a um cluster de Raspberry.

Espero que tenha gostado, e até a próxima!
