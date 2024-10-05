class MovieTheater
{
    const int Rows = 9;
    const int SeatsPerRow = 9;
    enum SeatStatus { Free, Booked }

    static SeatStatus[,] seats = new SeatStatus[Rows, SeatsPerRow];

    static void Main()
    {
        LoadSeats(seats);
        DisplaySeats(seats);

        while (true)
        {
            Console.WriteLine("Введите ряд 1-9 и место 1-9 чтобы забронировать место");

            string? input = Console.ReadLine();

            if (input?.ToLower() == "reset")
            {
                ResetSeats(seats);
                DisplaySeats(seats);
                continue;
            }

            string[] inputs = input?.Split(' ') ?? Array.Empty<string>();

            if (inputs.Length == 2 && 
                int.TryParse(inputs[0], out int row) && 
                int.TryParse(inputs[1], out int seat))
            {
                row--; seat--;
                if (row >= 0 && row < Rows && seat >= 0 && seat < SeatsPerRow && BookSeat(seats, row, seat))
                {
                    SaveSeats(seats);
                    DisplaySeats(seats);
                }
                else
                {
                    Console.WriteLine("Место занято");
                }
            }
            else
            {
                Console.WriteLine("Данные неверны");
            }
        }
    }

    static bool BookSeat(SeatStatus[,] seats, int row, int seat)
    {
        if (seats[row, seat] == SeatStatus.Free)
        {
            seats[row, seat] = SeatStatus.Booked;
            Console.WriteLine("Место забронировано");
            return true;
        }
        return false;
    }

    static void DisplaySeats(SeatStatus[,] seats)
    {
        Console.Write("  ");
        for (int j = 1; j <= SeatsPerRow; j++) Console.Write(j + " ");
        Console.WriteLine();

        for (int i = 0; i < Rows; i++)
        {
            Console.Write((i + 1) + " ");
            for (int j = 0; j < SeatsPerRow; j++)
            {
                Console.ForegroundColor = seats[i, j] == SeatStatus.Free ? ConsoleColor.Green : ConsoleColor.Red;
                Console.Write((seats[i, j] == SeatStatus.Free ? "■" : "x") + " ");
            }
            Console.ResetColor();
            Console.WriteLine();
        }
    }

    static void SaveSeats(SeatStatus[,] seats)
    {
        using (StreamWriter writer = new StreamWriter("seats.txt"))
        {
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < SeatsPerRow; j++)
                {
                    writer.Write((int)seats[i, j] + " ");
                }
                writer.WriteLine();
            }
        }
    }

    static void LoadSeats(SeatStatus[,] seats)
    {
        if (File.Exists("seats.txt"))
        {
            using (StreamReader reader = new StreamReader("seats.txt"))
            {
                for (int i = 0; i < Rows; i++)
                {
                    string[] seatData = reader.ReadLine()?.Split(' ') ?? Array.Empty<string>();
                    for (int j = 0; j < SeatsPerRow; j++)
                    {
                        seats[i, j] = (SeatStatus)int.Parse(seatData[j]);
                    }
                }
            }
        }
    }

    static void ResetSeats(SeatStatus[,] seats)
    {
        for (int i = 0; i < Rows; i++)
            for (int j = 0; j < SeatsPerRow; j++)
                seats[i, j] = SeatStatus.Free;

        SaveSeats(seats);
        Console.WriteLine("Все места свободны");
    }
}
