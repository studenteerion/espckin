SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET time_zone = "+00:00";
CREATE TABLE `accesso` (
    `id_accesso` int(11) NOT NULL,
  `nome_accesso` varchar(20) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
CREATE TABLE `macchina_professore` (
    `id_macchina` char(7) NOT NULL,
  `id_professore` varchar(10) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
CREATE TABLE `macchine` (
    `targa` char(7) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
CREATE TABLE `professore` (
    `id_professore` varchar(10) NOT NULL,
  `nome` varchar(30) NOT NULL,
  `cognome` varchar(30) NOT NULL,
  `mail` varchar(50) NOT NULL,
  `id_zona` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
ALTER TABLE `accesso`
  ADD PRIMARY KEY (`id_accesso`);
ALTER TABLE `macchina_professore`
  ADD PRIMARY KEY (`id_macchina`,`id_professore`),
  ADD KEY `id_macchina` (`id_macchina`),
  ADD KEY `id_professore` (`id_professore`),
  ADD KEY `id_professore_2` (`id_professore`);
ALTER TABLE `macchine`
  ADD PRIMARY KEY (`targa`);
ALTER TABLE `professore`
  ADD PRIMARY KEY (`id_professore`),
  ADD KEY `id_zona` (`id_zona`);
ALTER TABLE `macchina_professore`
  ADD CONSTRAINT `macchina_professore_ibfk_1` FOREIGN KEY (`id_macchina`) REFERENCES `macchine` (`targa`),
  ADD CONSTRAINT `macchina_professore_ibfk_2` FOREIGN KEY (`id_professore`) REFERENCES `professore` (`id_professore`);
ALTER TABLE `professore`
  ADD CONSTRAINT `professore_targa` FOREIGN KEY (`id_zona`) REFERENCES `accesso` (`id_accesso`);
