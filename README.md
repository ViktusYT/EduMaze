# Projekt - EduMaze

Eud Maze to eukacyjna gra typu labirynt, w której gracz wybiera zestaw pytań, a następnie musi pokonać labirynt odpowiadając na pytania. Ilość błędyncyh odpowiedzi jest mocno ograniczona :).

## Technologia

Projekt jest obecnie pisany w języku C# z wykorzystaniem .NET 5. Dodatkowo używam bibliotek SFML.NET do wyświetlania obrazu oraz JSON.NET do parsownia plików typu JSON.

## Szczegółowy opis gry

Gracz (po wyborze zestawu pytań) pojawia się w punkcie 0, 0 w proceduralnie wygenwrowanym labiryncie. Może poruszać się za pomocą strzałek lub popularnego WSADu. Musi przedostać się na przeciwległy kraniec labiryntu. Cały labirynt usiany jest znakami zapytania, po wejściu na niego pojawia się monit z pytaniem i maksymalnie czterema odpowiedziami. Po poprawnej opowiedzi znak zapytania oraz monit znkają i można kontynuować grę. Po złej odpowiedzi gracz traci jedno życie i może ponownie odpowiedzieć na to samo pytanie.

## Opis kodu

Główną częścią kody jest obecnie klasa Game. Tu znajdują się obiekt labiryntu, zestawu pytań i gracza. Klasa ma dwie ważne metody: Update i Render. Update aktualizuję logikę programu, a render w pętli wyświetla wszystkie obiekty. Mamy też bardzo rozbudowaną klasę Maze, która w raz z klasą MazeNode generuje i przechowuje labirynt, a także zarządza położeniem pytań i pozycją gracze. Klasa gracza to na razie przemieszczający się kwadrat z informacją o punkatch życia. Kolejną ważną klasą jest QuestionSet, który przechowuje i podaje pytania, sprawdza odpowiedź oraz wczytuje zestaw z pliku .JSON. W kodzie będzie znajdował się także machanizm warstw, podobny do warstw z GIMPA. Silnik w ten sposób będzie wiedział co jest na wierzchu, a co pod spodem. Istnieją równiez dwa interfejsy: IDrawable, który maja klasy zdolne do wyświetlania siebie oraz ISelectable, który mają obiekty na który można kliknąć myszą.