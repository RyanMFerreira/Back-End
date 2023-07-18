class Program
{
    static void Main()
    {
        int opcaoSelecionada = 0;

        while (true)
        {
            Console.Clear();

            Console.WriteLine("Snake Game\n");

            // Verifica a opção selecionada e a exibe visualmente
            Console.ForegroundColor = opcaoSelecionada == 0 ? ConsoleColor.Black : ConsoleColor.White;
            Console.BackgroundColor = opcaoSelecionada == 0 ? ConsoleColor.White : ConsoleColor.Black;
            Console.WriteLine("Iniciar Programa");

            Console.ForegroundColor = opcaoSelecionada == 1 ? ConsoleColor.Black : ConsoleColor.White;
            Console.BackgroundColor = opcaoSelecionada == 1 ? ConsoleColor.White : ConsoleColor.Black;
            Console.WriteLine("Fechar Programa");

            Console.ResetColor();

            Console.WriteLine("\nUtilize as setas (cima/baixo)\npara navegar entre as opções...\n\nv0.85");

            // Estrutura que armazenará os dados da tecla pressionada
            ConsoleKeyInfo teclaPressionada = Console.ReadKey();

            // Verifica a tecla pressionada e alterna entre as opções
            if (teclaPressionada.Key == ConsoleKey.UpArrow && opcaoSelecionada > 0)
            {
                opcaoSelecionada--;
            }
            else if (teclaPressionada.Key == ConsoleKey.DownArrow && opcaoSelecionada < 1)
            {
                opcaoSelecionada++;
            }
            else if (teclaPressionada.Key == ConsoleKey.Enter)
            {
                if (opcaoSelecionada == 0)
                {
                    IniciarPrograma();
                    break;
                }
                else if (opcaoSelecionada == 1)
                {
                    break;
                }
            }
        }
    }

    static void IniciarPrograma()
    {
        // Definindo o tamanho do mapa
        int larguraMapa = 32;
        int alturaMapa = 15;

        // Criando a cobra e posicionando-a no centro do mapa
        int[] cobraPosX = new int[alturaMapa*larguraMapa];
        int[] cobraPosY = new int[alturaMapa*larguraMapa];
        int tamanhoCobra = 3;
        cobraPosX[0] = larguraMapa / 2;
        cobraPosY[0] = alturaMapa / 2;

        // Posicionando a comida em uma posição aleatória no mapa
        Random posAleat = new Random();
        int comidaPosX = posAleat.Next(1, larguraMapa - 1);
        int comidaPosY = posAleat.Next(1, alturaMapa - 1);

        // Variáveis para controlar a direção da cobra
        int direcaoX = 0;
        int direcaoY = 0;

        // Contador para a pontuação
        int pontuacao = 0;

        // Variável Booleana para checar o fim do jogo
        bool GameOver = false;

        // Variável para verificar a vitória
        bool Win = false;

        while (true)
        {
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo teclaPressionada = Console.ReadKey();

                // Atualizando a direção da cobra com base na tecla pressionada
                if (teclaPressionada.Key == ConsoleKey.UpArrow && direcaoY != 1)
                {
                    direcaoX = 0;
                    direcaoY = -1;  
                }
                else if (teclaPressionada.Key == ConsoleKey.DownArrow && direcaoY != -1)
                {
                    direcaoX = 0;
                    direcaoY = 1;
                }
                else if (teclaPressionada.Key == ConsoleKey.LeftArrow && direcaoX != 1)
                {
                    direcaoX = -1;
                    direcaoY = 0; 
                }
                else if (teclaPressionada.Key == ConsoleKey.RightArrow && direcaoX != -1)
                {
                    direcaoX = 1;
                    direcaoY = 0;
                }
            }

            // Atualizando a posição da cobra com base na direção
            cobraPosX[0] += direcaoX; 
            cobraPosY[0] += direcaoY;

            // Percorre os segmentos da cobra e alterna os valores para o anterior (decrescente)
            for (int i = tamanhoCobra; i > 0; i--)
            {
                cobraPosX[i] = cobraPosX[i - 1];
                cobraPosY[i] = cobraPosY[i - 1];
            }

            // Verifica se a cobra colidiu consigo mesma
            for (int i = 3; i < tamanhoCobra; i++)
            {
                if (cobraPosX[0] == cobraPosX[i] && cobraPosY[0] == cobraPosY[i])
                {
                    GameOver = true;
                    break;
                }
            }

            // Verificando se a cobra colidiu com a parede do mapa
            if (cobraPosX[0] <= 0 || cobraPosX[0] >= larguraMapa - 1 ||
                cobraPosY[0] <= 0 || cobraPosY[0] >= alturaMapa - 1)
            {
                GameOver = true;
            }

            // Verifica se a vitória ocorreu
            if (pontuacao >= (alturaMapa - 2) * (larguraMapa - 2))
            {
                Win = true;
            }

            // Verifica se houve vitória ou derrota
            if (Win == true || GameOver == true)
            {
                CheckMenu(GameOver, Win);
                break;
            }

            // Verifica se a cobra comeu a comida
            if (cobraPosX[0] == comidaPosX && cobraPosY[0] == comidaPosY)
            {
                // Emite um som
                Console.Beep(1750, 80);

                // Adiciona uma nova pontuação e aumenta o tamanho da cobra
                pontuacao += 1;
                tamanhoCobra += 1;

                // Posiciona a comida em outra coordenada aleatória
                comidaPosX = posAleat.Next(1, larguraMapa - 1);
                comidaPosY = posAleat.Next(1, alturaMapa - 1);
            }

            // Limpa a tela
            Console.Clear();

            // Construindo o mapa
            string mapaString = "";

            for (int y = 0; y < alturaMapa; y++)
            {
                for (int x = 0; x < larguraMapa; x++)
                {
                    if (x == 0 || x == larguraMapa - 1 || y == 0 || y == alturaMapa - 1)
                    {
                        mapaString += "+"; // Desenha as bordas
                    }
                    else if (x == comidaPosX && y == comidaPosY)
                    {
                        mapaString += "$"; // Desenha a comida
                    }
                    else if (cobraPosX[0] == x && cobraPosY[0] == y)
                    {
                        mapaString += "@"; // Desenha a cabeça da cobra
                    }
                    else
                    {
                        // Percorre os segmentos da cobra e verifica se suas posições estão contidas em x e y
                        bool segmentoCobra = false;
                        for (int i = 0; i <= tamanhoCobra; i++)
                        {
                            if (cobraPosX[i] == x && cobraPosY[i] == y)
                            {
                                segmentoCobra = true;
                                break;
                            }
                        }
                        mapaString += segmentoCobra ? "o" : " "; //Desenha os segmentos do corpo da cobra e o espaço vazio no mapa
                    }
                } 

                mapaString += "\n"; // Adiciona uma quebra de linha no final de cada linha
            }

            // Desenha o mapa e o contador de pontuação
            Console.WriteLine(mapaString + "\nPontuação: " + pontuacao);

            // Aguarda um tempo para animação
            Thread.Sleep(150);
        }
    }

    static void CheckMenu(bool GameOver, bool Win)
    {
        Console.Clear();
        
        // Exibe o Menu de GameOver
        if (GameOver == true)
        {
            // Exibe a mensagem de GameOver e emite um som
            Console.WriteLine("Game Over!\n");
            Console.Beep(800, 200);
            Console.WriteLine("Pressione Enter para\nretornar ao menu...");

            ConsoleKeyInfo teclaPressionada = Console.ReadKey();
            if (teclaPressionada.Key == ConsoleKey.Enter)
            {
                Main();
            }
            else
            {
                CheckMenu(GameOver, Win);
            }
        }
        // Exibe o Menu de Vitória
        else if (Win == true)
        {
            Console.WriteLine("Parabéns, você venceu!\n\nPressione Enter para\nretornar ao menu...");
            ConsoleKeyInfo teclaPressionada = Console.ReadKey();
            if (teclaPressionada.Key == ConsoleKey.Enter)
            {
                Main();
            }
            else
            {
                CheckMenu(GameOver, Win);
            }
        }
    }
}
