-- MySQL dump 10.13  Distrib 8.0.20, for Win64 (x86_64)
--
-- Host: 127.0.0.1    Database: bdsde
-- ------------------------------------------------------
--
-- Table structure for table `aluno`
--
DROP DATABASE IF EXISTS bdsde;

CREATE DATABASE bdsde;

USE bdsde;

DROP TABLE IF EXISTS `aluno`;

CREATE TABLE `aluno` (
  `idaluno` int(11) NOT NULL AUTO_INCREMENT,
  `datacadastro` date DEFAULT NULL,
  `nome` varchar(100) DEFAULT NULL,
  `telefoneres` varchar(50) DEFAULT NULL,
  `telefonecel` varchar(50) DEFAULT NULL,
  `email` varchar(100) DEFAULT NULL,
  `observacao` varchar(255) DEFAULT NULL,
  `cpf` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`idaluno`)
) ENGINE=InnoDB AUTO_INCREMENT=6088 DEFAULT CHARSET=latin1;

--
-- Dumping data for table `aluno`
--

LOCK TABLES `aluno` WRITE;
/*!40000 ALTER TABLE `aluno` DISABLE KEYS */;
INSERT INTO `aluno` VALUES (1,'2021-12-30','NOME DO ALUNO 1','(14)3263-1234','(14)99123-1234',NULL,NULL,'12312312312');
/*!40000 ALTER TABLE `aluno` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `curso`
--

DROP TABLE IF EXISTS `curso`;
CREATE TABLE `curso` (
  `idcurso` int(11) NOT NULL AUTO_INCREMENT,
  `cursonome` varchar(100) NOT NULL,
  `descricao` varchar(255) DEFAULT NULL,
  `cargahoraria` int(11) NOT NULL,
  PRIMARY KEY (`idcurso`)
) ENGINE=InnoDB AUTO_INCREMENT=116 DEFAULT CHARSET=utf8;

--
-- Dumping data for table `curso`
--

LOCK TABLES `curso` WRITE;
/*!40000 ALTER TABLE `curso` DISABLE KEYS */;
INSERT INTO `curso` VALUES (1,'Operador de Microcomputador','Informática',160);
/*!40000 ALTER TABLE `curso` ENABLE KEYS */;
UNLOCK TABLES;


--
-- Table structure for table `usuario`
--

DROP TABLE IF EXISTS `usuario`;
CREATE TABLE `usuario` (
  `idusuario` int(11) NOT NULL AUTO_INCREMENT,
  `perfil` varchar(45) NOT NULL,
  `username` varchar(45) NOT NULL,
  `password` varchar(45) NOT NULL,
  `nomecompleto` varchar(100) NOT NULL,
  PRIMARY KEY (`idusuario`)
) ENGINE=InnoDB AUTO_INCREMENT=82 DEFAULT CHARSET=utf8;

--
-- Dumping data for table `usuario`
--

LOCK TABLES `usuario` WRITE;
INSERT INTO `usuario` VALUES (1,'coordenador','admin','123456','Administrador do Sistema'),(2,'professor','professor1','123456','Nome do Professor 1');
UNLOCK TABLES;

--
-- Table structure for table `funcionario`
--

DROP TABLE IF EXISTS `funcionario`;
CREATE TABLE `funcionario` (
  `idfuncionario` int(11) NOT NULL AUTO_INCREMENT,
  `nomecompleto` varchar(100) NOT NULL,
  `email` varchar(100) DEFAULT NULL,
  `rg` varchar(14) DEFAULT NULL,
  `cpf` varchar(21) DEFAULT NULL,
  `telefonecel` varchar(45) DEFAULT NULL,
  `telefoneres` varchar(45) DEFAULT NULL,
  `logradouro` varchar(80) DEFAULT NULL,
  `numero` varchar(12) DEFAULT NULL,
  `bairro` varchar(40) DEFAULT NULL,
  `cidade` varchar(40) DEFAULT NULL,
  `estado` varchar(30) DEFAULT NULL,
  `cep` varchar(10) DEFAULT NULL,
  `especialidade` varchar(255) DEFAULT NULL,
  `obs` varchar(255) DEFAULT NULL,
  `usuario_idusuario` int(11) DEFAULT NULL,
  PRIMARY KEY (`idfuncionario`),
  KEY `usuario_idusuario_idx1` (`usuario_idusuario`) USING BTREE,
  CONSTRAINT `fk_funcionario_usuario1` FOREIGN KEY (`usuario_idusuario`) REFERENCES `usuario` (`idusuario`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=68 DEFAULT CHARSET=utf8;

--
-- Dumping data for table `funcionario`
--

LOCK TABLES `funcionario` WRITE;
INSERT INTO `funcionario` VALUES (1,'Carlos Alexandre Cavalheiro','vouler@hotmail.com',NULL,NULL,'14 99894-1477','14 3264-1234','Rua do Funcinoário','XXX','Núcleo H. L. Zillo','Lençóis Paulista','SP','18685-000','Informática','- Operador de Microcomputador\r\n- Excel Avançado\r\n- Web Designer\r\n- Mantenedor Assistente de Microcomputador\r\n- Open Office\r\n- Linux Instalação e Configuração',2);
UNLOCK TABLES;

--
-- Table structure for table `sugestoes`
--

DROP TABLE IF EXISTS `sugestoes`;
CREATE TABLE `sugestoes` (
  `idsugestao` int(11) NOT NULL AUTO_INCREMENT,
  `tipo` varchar(45) DEFAULT NULL,
  `titulo` varchar(45) DEFAULT NULL,
  `descricao` text,
  `status` varchar(45) DEFAULT NULL,
  `usuario_idusuario` int(11) NOT NULL,
  PRIMARY KEY (`idsugestao`),
  KEY `fk_sugestoes_usuario1_idx` (`usuario_idusuario`),
  CONSTRAINT `fk_sugestoes_usuario1` FOREIGN KEY (`usuario_idusuario`) REFERENCES `usuario` (`idusuario`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8;

--
-- Table structure for table `turma`
--

DROP TABLE IF EXISTS `turma`;
CREATE TABLE `turma` (
  `idturma` int(11) NOT NULL AUTO_INCREMENT,
  `descricao` varchar(255) NOT NULL,
  `cliente` varchar(255) NOT NULL,
  `horario` varchar(100) NOT NULL,
  `datainicio` date DEFAULT NULL,
  `datafim` date DEFAULT NULL,
  `horasaula` decimal(10,1) NOT NULL,
  `segunda` bit(1) NOT NULL DEFAULT b'0',
  `terca` bit(1) NOT NULL DEFAULT b'0',
  `quarta` bit(1) NOT NULL DEFAULT b'0',
  `quinta` bit(1) NOT NULL DEFAULT b'0',
  `sexta` bit(1) NOT NULL DEFAULT b'0',
  `sabado` bit(1) NOT NULL DEFAULT b'0',
  `domingo` bit(1) NOT NULL DEFAULT b'0',
  `status` bit(1) NOT NULL DEFAULT b'0',
  `curso_idcurso` int(11) NOT NULL,
  `funcionario_idfuncionario` int(11) NOT NULL,
  `responsavel` varchar(100) NOT NULL,
  `proposta_ano` varchar(45) DEFAULT NULL,
  `local_realizacao` varchar(100) DEFAULT NULL,
  `atendimento` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`idturma`),
  KEY `fk_turma_curso1_idx` (`curso_idcurso`),
  KEY `fk_turma_funcionario1_idx` (`funcionario_idfuncionario`) USING BTREE,
  CONSTRAINT `fk_turma_curso1` FOREIGN KEY (`curso_idcurso`) REFERENCES `curso` (`idcurso`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_turma_funcionario1` FOREIGN KEY (`funcionario_idfuncionario`) REFERENCES `funcionario` (`idfuncionario`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=335 DEFAULT CHARSET=utf8;

--
-- Table structure for table `matricula`
--

DROP TABLE IF EXISTS `matricula`;
CREATE TABLE `matricula` (
  `idmatricula` int(10) unsigned zerofill NOT NULL AUTO_INCREMENT,
  `datamatricula` date DEFAULT NULL,
  `situacao` varchar(45) DEFAULT NULL,
  `datasituacao` date DEFAULT NULL,
  `aluno_idaluno` int(11) NOT NULL,
  `turma_idturma` int(11) NOT NULL,
  PRIMARY KEY (`idmatricula`),
  KEY `fk_matricula_aluno1_idx` (`aluno_idaluno`),
  KEY `fk_matricula_turma1_idx` (`turma_idturma`),
  CONSTRAINT `fk_matricula_aluno1` FOREIGN KEY (`aluno_idaluno`) REFERENCES `aluno` (`idaluno`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_matricula_turma1` FOREIGN KEY (`turma_idturma`) REFERENCES `turma` (`idturma`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=5417 DEFAULT CHARSET=utf8;


--
-- Table structure for table `unidadecurricular`
--

DROP TABLE IF EXISTS `unidadecurricular`;
CREATE TABLE `unidadecurricular` (
  `idunidadecurricular` int(11) NOT NULL AUTO_INCREMENT,
  `unidadecurricular` varchar(45) NOT NULL,
  `cargahoraria` int(11) NOT NULL,
  `curso_idcurso` int(11) NOT NULL,
  `sigla` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`idunidadecurricular`),
  KEY `fk_unidade_curricular_curso_idx` (`curso_idcurso`),
  CONSTRAINT `fk_unidade_curricular_curso` FOREIGN KEY (`curso_idcurso`) REFERENCES `curso` (`idcurso`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=290 DEFAULT CHARSET=utf8;

--
-- Dumping data for table `unidadecurricular`
--

LOCK TABLES `unidadecurricular` WRITE;
INSERT INTO `unidadecurricular` VALUES (1,'FUNDAMENTOS DE INFORMÁTICA',30,1,'FHW'),(2,'Fundamentos de Sistema Operacional',40,1,NULL),(3,'Aplicativos Office e Internet',90,1,NULL);
UNLOCK TABLES;


--
-- Table structure for table `notas`
--

DROP TABLE IF EXISTS `notas`;
CREATE TABLE `notas` (
  `idnotas` int(11) NOT NULL AUTO_INCREMENT,
  `nota` decimal(10,0) DEFAULT NULL,
  `matricula_idmatricula` int(10) unsigned zerofill NOT NULL,
  `unidadecurricular_idunidadecurricular` int(11) NOT NULL,
  PRIMARY KEY (`idnotas`),
  KEY `fk_notas_matricula1_idx` (`matricula_idmatricula`),
  KEY `fk_notas_unidadecurricular1_idx` (`unidadecurricular_idunidadecurricular`),
  CONSTRAINT `fk_notas_matricula1` FOREIGN KEY (`matricula_idmatricula`) REFERENCES `matricula` (`idmatricula`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `fk_notas_unidadecurricular1` FOREIGN KEY (`unidadecurricular_idunidadecurricular`) REFERENCES `unidadecurricular` (`idunidadecurricular`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=9235 DEFAULT CHARSET=utf8;


--
-- Table structure for table `aproveitamento`
--

DROP TABLE IF EXISTS `aproveitamento`;
CREATE TABLE `aproveitamento` (
  `idaproveitamento` int(11) NOT NULL AUTO_INCREMENT,
  `matricula_idmatricula` int(10) unsigned zerofill NOT NULL,
  `unidadecurricular_idunidadecurricular` int(11) NOT NULL,
  PRIMARY KEY (`idaproveitamento`),
  KEY `fk_aproveitamento_matricula_idx` (`matricula_idmatricula`),
  KEY `fk_aproveitamento_unidadecurricular_idx` (`unidadecurricular_idunidadecurricular`),
  CONSTRAINT `fk_aproveitamento_matricula` FOREIGN KEY (`matricula_idmatricula`) REFERENCES `matricula` (`idmatricula`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_aproveitamento_unidadecurricular` FOREIGN KEY (`unidadecurricular_idunidadecurricular`) REFERENCES `unidadecurricular` (`idunidadecurricular`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=176 DEFAULT CHARSET=utf8;


--
-- Table structure for table `diario`
--

DROP TABLE IF EXISTS `diario`;
CREATE TABLE `diario` (
  `iddiario` int(11) NOT NULL AUTO_INCREMENT,
  `conteudo` varchar(200) NOT NULL,
  `ocorrencia` varchar(200) DEFAULT NULL,
  `data` date NOT NULL,
  `turma_idturma` int(11) NOT NULL,
  `unidade_curricular_idunidadecurricular` int(11) NOT NULL,
  `hora_aula_dia` decimal(10,1) NOT NULL,
  PRIMARY KEY (`iddiario`),
  KEY `fk_diario_turma1_idx` (`turma_idturma`),
  KEY `unidade_curricular_idunidadecurricular` (`unidade_curricular_idunidadecurricular`),
  KEY `iddiario` (`iddiario`),
  KEY `uk_diario_turma` (`turma_idturma`),
  CONSTRAINT `diario_fk1` FOREIGN KEY (`unidade_curricular_idunidadecurricular`) REFERENCES `unidadecurricular` (`idunidadecurricular`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `fk_diario_turma1` FOREIGN KEY (`turma_idturma`) REFERENCES `turma` (`idturma`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=12154 DEFAULT CHARSET=utf8;

--
-- Table structure for table `frequencia`
--

DROP TABLE IF EXISTS `frequencia`;
CREATE TABLE `frequencia` (
  `idfrequencia` bigint(20) NOT NULL AUTO_INCREMENT,
  `horaaula` decimal(10,1) NOT NULL,
  `presenca` varchar(2) NOT NULL,
  `matricula_idmatricula` int(10) unsigned zerofill NOT NULL,
  `diario_iddiario` int(11) DEFAULT NULL,
  PRIMARY KEY (`idfrequencia`),
  KEY `fk_frequencia_matricula1_idx` (`matricula_idmatricula`),
  KEY `diario_iddiario` (`diario_iddiario`),
  CONSTRAINT `fk_frequencia_matricula1` FOREIGN KEY (`matricula_idmatricula`) REFERENCES `matricula` (`idmatricula`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `frequencia_fk1` FOREIGN KEY (`diario_iddiario`) REFERENCES `diario` (`iddiario`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=187137 DEFAULT CHARSET=utf8;


-- Dump completed on 2021-12-30  7:40:27
