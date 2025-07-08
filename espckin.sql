-- phpMyAdmin SQL Dump
-- version 4.5.1
-- http://www.phpmyadmin.net
--
-- Host: 127.0.0.1
-- Creato il: Gen 14, 2025 alle 15:36
-- Versione del server: 10.1.10-MariaDB
-- Versione PHP: 7.0.2

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `espckin`
--

-- --------------------------------------------------------

--
-- Struttura della tabella `accesso`
--

CREATE TABLE `accesso` (
  `id_accesso` int(11) NOT NULL,
  `zona_accesso` varchar(20) NOT NULL,
  `ip` varchar(50) NOT NULL,
  `descrizione` text(500),
  `coordinates` varchar(100)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dump dei dati per la tabella `accesso`
--

INSERT INTO `accesso` (`id_accesso`, `zona_accesso`) VALUES
(0, 'fronte'),
(1, 'retro');

-- --------------------------------------------------------

--
-- Struttura della tabella `macchina_professore`
--

CREATE TABLE `macchina_professore` (
  `id_macchina` char(7) NOT NULL,
  `id_professore` varchar(10) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dump dei dati per la tabella `macchina_professore`
--

INSERT INTO `macchina_professore` (`id_macchina`, `id_professore`) VALUES
('aa111aa', 'mat'),
('aa111aa', 'ciao'),
('bb222bb', 'mat');

-- --------------------------------------------------------

--
-- Struttura della tabella `macchine`
--

CREATE TABLE `macchine` (
  `targa` char(7) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dump dei dati per la tabella `macchine`
--

INSERT INTO `macchine` (`targa`) VALUES
('aa111aa'),
('bb222bb');

-- --------------------------------------------------------

--
-- Struttura della tabella `professore`
--

CREATE TABLE `professore` (
  `id_professore` varchar(10) NOT NULL,
  `nome` varchar(30) NOT NULL,
  `cognome` varchar(30) NOT NULL,
  `mail` varchar(50) NOT NULL,
  `id_zona` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dump dei dati per la tabella `professore`
--

INSERT INTO `professore` (`id_professore`, `nome`, `cognome`, `mail`, `id_zona`) VALUES
('mat', 'Matilde', 'Ravasio', 'matilde.ravasio.studente@itispaleocapa.it', 1),
('ciao', 'Fabio', 'Colombo', 'colombo.fabio.studente@itispaleocapa.it', 0);


CREATE TABLE utenti (
  id INT AUTO_INCREMENT PRIMARY KEY,
  username VARCHAR(50) NOT NULL UNIQUE,
  api_key VARCHAR(50) NOT NULL UNIQUE,
  ruolo VARCHAR(20) DEFAULT 'user' -- Esempio: frontend, prof, admin
);

INSERT INTO utenti (username, api_key, ruolo)
VALUES 
('frontend', 'BJFLMJTARU', 'frontend'),
('prof', 'VYPPMADJXD', 'prof');

--
-- Indici per le tabelle scaricate
--

--
-- Indici per le tabelle `accesso`
--
ALTER TABLE `accesso`
  ADD PRIMARY KEY (`id_accesso`);

--
-- Indici per le tabelle `macchina_professore`
--
ALTER TABLE `macchina_professore`
  ADD PRIMARY KEY (`id_macchina`,`id_professore`),
  ADD KEY `id_macchina` (`id_macchina`),
  ADD KEY `id_professore` (`id_professore`),
  ADD KEY `id_professore_2` (`id_professore`);

--
-- Indici per le tabelle `macchine`
--
ALTER TABLE `macchine`
  ADD PRIMARY KEY (`targa`);

--
-- Indici per le tabelle `professore`
--
ALTER TABLE `professore`
  ADD PRIMARY KEY (`id_professore`),
  ADD KEY `id_zona` (`id_zona`);

--
-- Limiti per le tabelle scaricate
--

--
-- Limiti per la tabella `macchina_professore`
--
ALTER TABLE `macchina_professore`
  ADD CONSTRAINT `macchina_professore_ibfk_1` FOREIGN KEY (`id_macchina`) REFERENCES `macchine` (`targa`),
  ADD CONSTRAINT `macchina_professore_ibfk_2` FOREIGN KEY (`id_professore`) REFERENCES `professore` (`id_professore`);

--
-- Limiti per la tabella `professore`
--
ALTER TABLE `professore`
  ADD CONSTRAINT `professore_targa` FOREIGN KEY (`id_zona`) REFERENCES `accesso` (`id_accesso`);

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
