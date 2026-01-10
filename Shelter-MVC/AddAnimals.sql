-- Skrypt do ręcznego dodania zwierząt do bazy danych
-- Uruchom ten skrypt w SQL Server Management Studio lub przez sqlcmd

-- Najpierw usuń kolumnę SpeciesId jeśli istnieje (jeśli wcześniej usunęliśmy Species)
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Animals]') AND name = 'SpeciesId')
BEGIN
    -- Najpierw usuń indeks jeśli istnieje
    IF EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Animals_SpeciesId' AND object_id = OBJECT_ID(N'[dbo].[Animals]'))
    BEGIN
        DROP INDEX [IX_Animals_SpeciesId] ON [dbo].[Animals];
        PRINT 'Indeks IX_Animals_SpeciesId został usunięty.';
    END
    
    -- Usuń foreign key constraint jeśli istnieje
    IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Animals_Species_SpeciesId')
    BEGIN
        ALTER TABLE [dbo].[Animals] DROP CONSTRAINT [FK_Animals_Species_SpeciesId];
        PRINT 'Foreign key constraint FK_Animals_Species_SpeciesId został usunięty.';
    END
    
    -- Teraz usuń kolumnę SpeciesId
    ALTER TABLE [dbo].[Animals] DROP COLUMN [SpeciesId];
    PRINT 'Kolumna SpeciesId została usunięta.';
END
ELSE
BEGIN
    PRINT 'Kolumna SpeciesId nie istnieje - pomijam usuwanie.';
END
GO

-- Sprawdź czy kolumna AdoptedByUserId istnieje, jeśli nie - dodaj ją
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Animals]') AND name = 'AdoptedByUserId')
BEGIN
    ALTER TABLE [dbo].[Animals] ADD [AdoptedByUserId] NVARCHAR(MAX) NULL;
    PRINT 'Kolumna AdoptedByUserId została dodana.';
END
GO

-- Najpierw usuń istniejące zwierzęta (opcjonalnie - usuń komentarz jeśli chcesz)
-- DELETE FROM Animals;

-- Dodaj zwierzęta
INSERT INTO Animals (Name, Age, Description, PhotoUrl, IsAdopted, AdoptedByUserId) VALUES
-- Psy
('Bella', 3, 'Urocza suczka rasy Australian Shepherd. Bardzo energiczna i przyjazna, uwielbia zabawę i długie spacery. Idealna dla aktywnych rodzin.', '/picture/australian-shepherd-5902417_1280.jpg', 0, NULL),
('Max', 2, 'Wesoły Corgi o przyjaznym usposobieniu. Uwielbia towarzystwo ludzi i innych zwierząt. Doskonały towarzysz dla całej rodziny.', '/picture/corgi-6673343_1280.jpg', 0, NULL),
('Luna', 4, 'Spokojna i zrównoważona suczka rasy Dachshund. Idealna dla osób szukających spokojnego towarzysza. Bardzo lojalna i oddana.', '/picture/dachshund-1519374_1280.jpg', 0, NULL),
('Rocky', 5, 'Aktywny Jack Russell Terrier pełen energii. Wymaga dużo ruchu i uwagi. Doskonały dla osób aktywnych fizycznie.', '/picture/jack-russell-2029214_1280.jpg', 0, NULL),
('Charlie', 1, 'Młody, pełen życia pies. Bardzo towarzyski i chętny do nauki. Idealny dla rodzin z dziećmi.', '/picture/dog-1787835_1280.jpg', 0, NULL),
('Daisy', 3, 'Przyjazna suczka o łagodnym charakterze. Uwielbia pieszczoty i spokojne spacery. Doskonała dla osób starszych.', '/picture/dog-1839808_1280.jpg', 0, NULL),
('Buddy', 2, 'Wesoły i energiczny pies. Bardzo inteligentny i szybko się uczy. Idealny towarzysz dla aktywnych osób.', '/picture/dog-1854119_1280.jpg', 0, NULL),
('Milo', 4, 'Spokojny i zrównoważony pies o przyjaznym usposobieniu. Uwielbia zabawę i długie spacery. Doskonały dla rodzin.', '/picture/dog-2561134_1280.jpg', 0, NULL),
('Lola', 3, 'Urocza suczka pełna energii. Bardzo towarzyska i przyjazna. Idealna dla osób szukających aktywnego towarzysza.', '/picture/dog-3277414_1280.jpg', 0, NULL),
('Zeus', 5, 'Duży, spokojny pies o łagodnym charakterze. Bardzo lojalny i oddany. Doskonały stróż i towarzysz.', '/picture/dog-5213090_1280.jpg', 0, NULL),
('Ruby', 2, 'Młoda, energiczna suczka. Uwielbia zabawę i aktywność fizyczną. Idealna dla osób aktywnych.', '/picture/dog-6389277_1280.jpg', 0, NULL),
('Oscar', 4, 'Przyjazny pies o spokojnym usposobieniu. Uwielbia pieszczoty i spokojne chwile. Doskonały towarzysz.', '/picture/dog-6977210_1280.jpg', 0, NULL),
('Lucy', 3, 'Urocza suczka pełna życia. Bardzo towarzyska i chętna do zabawy. Idealna dla rodzin z dziećmi.', '/picture/dog-7630252_1280.jpg', 0, NULL),
('Cooper', 2, 'Młody, energiczny pies. Bardzo inteligentny i szybko się uczy. Doskonały dla osób aktywnych.', '/picture/dog-8272860_1280.jpg', 0, NULL),
('Bella', 1, 'Mała, urocza suczka. Pełna energii i chęci do zabawy. Idealna dla rodzin z dziećmi.', '/picture/puppy-1207816_1280.jpg', 0, NULL),
('Max', 1, 'Młody, wesoły szczeniak. Bardzo towarzyski i chętny do nauki. Doskonały towarzysz dla całej rodziny.', '/picture/puppy-4608266_1280.jpg', 0, NULL),
-- Koty
('Whiskers', 2, 'Spokojny i zrównoważony kot. Uwielbia pieszczoty i spokojne chwile. Idealny dla osób szukających spokojnego towarzysza.', '/picture/cat-111793_1280.jpg', 0, NULL),
('Mia', 3, 'Urocza kotka o przyjaznym usposobieniu. Bardzo towarzyska i chętna do zabawy. Doskonała dla rodzin.', '/picture/cat-1192026_1280.jpg', 0, NULL),
('Oliver', 4, 'Spokojny i zrównoważony kot. Uwielbia spokojne chwile i pieszczoty. Idealny dla osób starszych.', '/picture/cat-2528935_1280.jpg', 0, NULL),
('Lily', 2, 'Młoda, energiczna kotka. Pełna życia i chęci do zabawy. Idealna dla aktywnych rodzin.', '/picture/cat-3113513_1280.jpg', 0, NULL),
('Charlie', 3, 'Przyjazny kot o łagodnym charakterze. Uwielbia towarzystwo ludzi. Doskonały towarzysz.', '/picture/cat-339400_1280.jpg', 0, NULL),
('Sophie', 4, 'Spokojna i zrównoważona kotka. Idealna dla osób szukających spokojnego towarzysza. Bardzo lojalna.', '/picture/cat-6572630_1280.jpg', 0, NULL),
('Simba', 2, 'Młody, energiczny kot. Pełen życia i chęci do zabawy. Idealny dla rodzin z dziećmi.', '/picture/simba-8618301_1280.jpg', 0, NULL),
('Luna', 3, 'Urocza kotka o przyjaznym usposobieniu. Bardzo towarzyska i chętna do pieszczot. Doskonała dla całej rodziny.', '/picture/cat-7094808_1280.jpg', 0, NULL),
('Max', 4, 'Spokojny i zrównoważony kot. Uwielbia spokojne chwile i pieszczoty. Idealny dla osób starszych.', '/picture/cat-8540772_1280.jpg', 0, NULL),
-- Inne zwierzęta
('Nibbles', 1, 'Mała, urocza fretka pełna energii. Bardzo towarzyska i chętna do zabawy. Wymaga dużo uwagi i aktywności.', '/picture/ferret-1215159_1280.jpg', 0, NULL),
('Peanut', 2, 'Przyjazna świnka morska o łagodnym charakterze. Idealna dla dzieci. Uwielbia pieszczoty i spokojne chwile.', '/picture/guinea-pig-2146091_1280.jpg', 0, NULL),
('Remi', 1, 'Mały, inteligentny szczurek. Bardzo towarzyski i chętny do zabawy. Idealny dla osób szukających małego towarzysza.', '/picture/rat-home-2899356_1280.jpg', 0, NULL);

PRINT 'Dodano 28 zwierząt do bazy danych.';
