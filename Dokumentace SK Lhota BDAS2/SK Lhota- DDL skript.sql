--------------------------------------------------------
--  File created - sobota-prosince-06-2025   
--------------------------------------------------------
--------------------------------------------------------
--  DDL for Type OBJECTS_LIST
--------------------------------------------------------

  CREATE OR REPLACE EDITIONABLE TYPE "ST72870"."OBJECTS_LIST" IS TABLE OF VARCHAR2(32767)

/
--------------------------------------------------------
--  DDL for Type REPORT_TEMPLATE
--------------------------------------------------------

  CREATE OR REPLACE EDITIONABLE TYPE "ST72870"."REPORT_TEMPLATE" AS OBJECT (reportType                 		INTEGER, useDescriptionInfo         		INTEGER, useQuantitativeInfo         		INTEGER, useDiagrams                 		INTEGER, useTableColumns             		INTEGER, useTableColumnsComments     		INTEGER, useTableIndexes             		INTEGER, useTableConstraints         		INTEGER, useTableFKReferringTo       		INTEGER, useTableFKReferredFrom      		INTEGER, useEntityAttributes         		INTEGER, useEntityAttributesComments 		INTEGER, useEntityConstraints        		INTEGER, useEntityIdentifiers      			INTEGER, useEntityRelationships      		INTEGER, useEntityIncomingProcesses  		INTEGER, useEntityOutgoingProcesses  		INTEGER, useDomainConstraints        		INTEGER, useDomainUsedInTables       		INTEGER, useDomainUsedInEntities     		INTEGER, useSTAttributes             		INTEGER, useSTAttributesComments     		INTEGER, useSTMethods                		INTEGER, useSTUsedInTables           		INTEGER, useSTUsedInEntities         		INTEGER, useCTUsedInTables           		INTEGER, useCTUsedInEntities         		INTEGER, useCTUsedInStructuredTypes  		INTEGER, useDTUsedInTables           		INTEGER, useDTUsedInEntities         		INTEGER, useDTUsedInStructuredTypes  		INTEGER, useSTUsedInStructuredTypes  		INTEGER, useDomainUsedInStructuredTypes  	INTEGER, useCRImpactedObjects        		INTEGER, useMRImpactedObjects        		INTEGER)

/
--------------------------------------------------------
--  DDL for Sequence SEKV_CLENKLUBU
--------------------------------------------------------

   CREATE SEQUENCE  "ST72870"."SEKV_CLENKLUBU"  MINVALUE 1 MAXVALUE 9999999999999999999999999999 INCREMENT BY 1 START WITH 175 NOCACHE  ORDER  NOCYCLE  NOKEEP  NOSCALE  GLOBAL ;
--------------------------------------------------------
--  DDL for Sequence SEKV_HRAC_CLENKLUBU
--------------------------------------------------------

   CREATE SEQUENCE  "ST72870"."SEKV_HRAC_CLENKLUBU"  MINVALUE 1 MAXVALUE 9999999999999999999999999999 INCREMENT BY 1 START WITH 41 NOCACHE  ORDER  NOCYCLE  NOKEEP  NOSCALE  GLOBAL ;
--------------------------------------------------------
--  DDL for Sequence SEKV_LOG_TABLE
--------------------------------------------------------

   CREATE SEQUENCE  "ST72870"."SEKV_LOG_TABLE"  MINVALUE 1 MAXVALUE 9999999999999999999999999999 INCREMENT BY 1 START WITH 2496 NOCACHE  ORDER  NOCYCLE  NOKEEP  NOSCALE  GLOBAL ;
--------------------------------------------------------
--  DDL for Sequence SEKV_OBSAH
--------------------------------------------------------

   CREATE SEQUENCE  "ST72870"."SEKV_OBSAH"  MINVALUE 1 MAXVALUE 9999999999999999999999999999 INCREMENT BY 1 START WITH 61 NOCACHE  ORDER  NOCYCLE  NOKEEP  NOSCALE  GLOBAL ;
--------------------------------------------------------
--  DDL for Sequence SEKV_OPATRENI
--------------------------------------------------------

   CREATE SEQUENCE  "ST72870"."SEKV_OPATRENI"  MINVALUE 1 MAXVALUE 9999999999999999999999999999 INCREMENT BY 1 START WITH 40 NOCACHE  ORDER  NOCYCLE  NOKEEP  NOSCALE  GLOBAL ;
--------------------------------------------------------
--  DDL for Sequence SEKV_POZICE
--------------------------------------------------------

   CREATE SEQUENCE  "ST72870"."SEKV_POZICE"  MINVALUE 1 MAXVALUE 9999999999999999999999999999 INCREMENT BY 1 START WITH 5 NOCACHE  ORDER  NOCYCLE  NOKEEP  NOSCALE  GLOBAL ;
--------------------------------------------------------
--  DDL for Sequence SEKV_ROLE
--------------------------------------------------------

   CREATE SEQUENCE  "ST72870"."SEKV_ROLE"  MINVALUE 1 MAXVALUE 9999999999999999999999999999 INCREMENT BY 1 START WITH 5 NOCACHE  NOORDER  NOCYCLE  NOKEEP  NOSCALE  GLOBAL ;
--------------------------------------------------------
--  DDL for Sequence SEKV_SOUTEZ
--------------------------------------------------------

   CREATE SEQUENCE  "ST72870"."SEKV_SOUTEZ"  MINVALUE 1 MAXVALUE 9999999999999999999999999999 INCREMENT BY 1 START WITH 17 NOCACHE  ORDER  NOCYCLE  NOKEEP  NOSCALE  GLOBAL ;
--------------------------------------------------------
--  DDL for Sequence SEKV_SPONZOR
--------------------------------------------------------

   CREATE SEQUENCE  "ST72870"."SEKV_SPONZOR"  MINVALUE 1 MAXVALUE 9999999999999999999999999999 INCREMENT BY 1 START WITH 57 NOCACHE  ORDER  NOCYCLE  NOKEEP  NOSCALE  GLOBAL ;
--------------------------------------------------------
--  DDL for Sequence SEKV_STAVZAPASU
--------------------------------------------------------

   CREATE SEQUENCE  "ST72870"."SEKV_STAVZAPASU"  MINVALUE 1 MAXVALUE 9999999999999999999999999999 INCREMENT BY 1 START WITH 3 NOCACHE  ORDER  NOCYCLE  NOKEEP  NOSCALE  GLOBAL ;
--------------------------------------------------------
--  DDL for Sequence SEKV_TRENER_CLENKLUBU
--------------------------------------------------------

   CREATE SEQUENCE  "ST72870"."SEKV_TRENER_CLENKLUBU"  MINVALUE 1 MAXVALUE 9999999999999999999999999999 INCREMENT BY 1 START WITH 1 NOCACHE  ORDER  NOCYCLE  NOKEEP  NOSCALE  GLOBAL ;
--------------------------------------------------------
--  DDL for Sequence SEKV_TRENINK
--------------------------------------------------------

   CREATE SEQUENCE  "ST72870"."SEKV_TRENINK"  MINVALUE 1 MAXVALUE 9999999999999999999999999999 INCREMENT BY 1 START WITH 43 NOCACHE  ORDER  NOCYCLE  NOKEEP  NOSCALE  GLOBAL ;
--------------------------------------------------------
--  DDL for Sequence SEKV_TYPSOUTEZE
--------------------------------------------------------

   CREATE SEQUENCE  "ST72870"."SEKV_TYPSOUTEZE"  MINVALUE 1 MAXVALUE 9999999999999999999999999999 INCREMENT BY 1 START WITH 6 NOCACHE  ORDER  NOCYCLE  NOKEEP  NOSCALE  GLOBAL ;
--------------------------------------------------------
--  DDL for Sequence SEKV_UZIVATELSKE_UCTY
--------------------------------------------------------

   CREATE SEQUENCE  "ST72870"."SEKV_UZIVATELSKE_UCTY"  MINVALUE 1 MAXVALUE 9999999999999999999999999999 INCREMENT BY 1 START WITH 161 CACHE 20 NOORDER  NOCYCLE  NOKEEP  NOSCALE  GLOBAL ;
--------------------------------------------------------
--  DDL for Sequence SEKV_ZAPAS
--------------------------------------------------------

   CREATE SEQUENCE  "ST72870"."SEKV_ZAPAS"  MINVALUE 1 MAXVALUE 9999999999999999999999999999 INCREMENT BY 1 START WITH 53 NOCACHE  ORDER  NOCYCLE  NOKEEP  NOSCALE  GLOBAL ;
--------------------------------------------------------
--  DDL for Table BINARNI_OBSAH
--------------------------------------------------------

  CREATE TABLE "ST72870"."BINARNI_OBSAH" 
   (	"IDBINARNIOBSAH" NUMBER(*,0), 
	"NAZEVSOUBORU" VARCHAR2(50 BYTE), 
	"TYPSOUBORU" VARCHAR2(50 BYTE), 
	"PRIPONASOUBORU" VARCHAR2(10 BYTE), 
	"OBSAH" BLOB, 
	"DATUMNAHRANI" DATE DEFAULT SYSDATE, 
	"DATUMMODIFIKACE" DATE, 
	"OPERACE" VARCHAR2(50 BYTE), 
	"IDUZIVATELSKYUCET" NUMBER(*,0)
   ) SEGMENT CREATION IMMEDIATE 
  PCTFREE 10 PCTUSED 40 INITRANS 1 MAXTRANS 255 
 NOCOMPRESS LOGGING
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "STUDENTI" 
 LOB ("OBSAH") STORE AS SECUREFILE (
  TABLESPACE "STUDENTI" ENABLE STORAGE IN ROW CHUNK 8192
  NOCACHE LOGGING  NOCOMPRESS  KEEP_DUPLICATES 
  STORAGE(INITIAL 106496 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)) ;

   COMMENT ON COLUMN "ST72870"."BINARNI_OBSAH"."IDBINARNIOBSAH" IS 'Atribut idBinarniObsah bude sloužit jako UID a bude nabývat èísel od 1 výše';
   COMMENT ON TABLE "ST72870"."BINARNI_OBSAH"  IS 'Tabulka BINARNI_OBSAH slouží  k ukládání binárních souborù v databázi';
--------------------------------------------------------
--  DDL for Table CLENOVE_KLUBU
--------------------------------------------------------

  CREATE TABLE "ST72870"."CLENOVE_KLUBU" 
   (	"IDCLENKLUBU" NUMBER(*,0), 
	"JMENO" VARCHAR2(20 BYTE), 
	"PRIJMENI" VARCHAR2(20 BYTE), 
	"TYPCLENA" VARCHAR2(20 BYTE), 
	"TELEFONNICISLO" VARCHAR2(12 BYTE), 
	"RODNE_CISLO" VARCHAR2(10 BYTE)
   ) SEGMENT CREATION IMMEDIATE 
  PCTFREE 10 PCTUSED 40 INITRANS 1 MAXTRANS 255 
 NOCOMPRESS LOGGING
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "STUDENTI" ;

   COMMENT ON COLUMN "ST72870"."CLENOVE_KLUBU"."IDCLENKLUBU" IS 'Atribut idClenKlubu bude sloužit jako UID a bude nabývat èísel od 1 výše';
   COMMENT ON TABLE "ST72870"."CLENOVE_KLUBU"  IS 'Tabulka CLENOVE_KLUBU slouží jako supertyp pro tabulky "HRACI"  a "TRENERI". Tabulka CLENOVE_KLUBU slouží k uchování osobních údajù o hráèích a trenérech.
Jsou zde øešena integritní omezení IO7, IO10';
--------------------------------------------------------
--  DDL for Table DISCIPLINARNI_OPATRENI
--------------------------------------------------------

  CREATE TABLE "ST72870"."DISCIPLINARNI_OPATRENI" 
   (	"IDOPATRENI" NUMBER(*,0), 
	"DATUMOPATRENI" DATE, 
	"DELKATRESTU" NUMBER(*,0), 
	"DUVOD" VARCHAR2(20 BYTE)
   ) SEGMENT CREATION IMMEDIATE 
  PCTFREE 10 PCTUSED 40 INITRANS 1 MAXTRANS 255 
 NOCOMPRESS LOGGING
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "STUDENTI" ;

   COMMENT ON COLUMN "ST72870"."DISCIPLINARNI_OPATRENI"."IDOPATRENI" IS 'Atribut idOpatreni bude sloužit jako UID a bude nabývat èísel od 1 výše';
   COMMENT ON TABLE "ST72870"."DISCIPLINARNI_OPATRENI"  IS 'Tabulka DISCIPLINARNI_OPATRENI slouží pro uchování disciplinárních opatøení, která jsou udìlována hráèùm. Je zde øešeno integritní omezení IO7';
--------------------------------------------------------
--  DDL for Table HRACI
--------------------------------------------------------

  CREATE TABLE "ST72870"."HRACI" 
   (	"IDCLENKLUBU" NUMBER(*,0), 
	"POCETVSTRELENYCHGOLU" NUMBER(38,0), 
	"IDOPATRENI" NUMBER(*,0), 
	"ID_POZICE" NUMBER, 
	"POCET_ZLUTYCH_KARET" NUMBER(3,0), 
	"POCET_CERVENYCH_KARET" NUMBER(3,0)
   ) SEGMENT CREATION IMMEDIATE 
  PCTFREE 10 PCTUSED 40 INITRANS 1 MAXTRANS 255 
 NOCOMPRESS LOGGING
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "STUDENTI" ;

   COMMENT ON COLUMN "ST72870"."HRACI"."IDCLENKLUBU" IS 'Atribut idClenKlubu bude sloužit jako UID a bude nabývat èísel od 1 výše';
   COMMENT ON TABLE "ST72870"."HRACI"  IS 'Tabulka HRACI je subtypem CLENOVE_KLUBU, slouží k uchování specifických údajù pro hráèe.
Je zde øešeno integritní omezení IO8.
Jsou zde øešena procedurální pravidla PP1, PP4';
--------------------------------------------------------
--  DDL for Table KONTRAKTY
--------------------------------------------------------

  CREATE TABLE "ST72870"."KONTRAKTY" 
   (	"DATUMZACATKU" DATE, 
	"DATUMKONCE" DATE, 
	"PLAT" NUMBER(*,0), 
	"CISLONAAGENTA" NUMBER(*,0), 
	"VYSTUPNIKLAUZULE" NUMBER(*,0), 
	"IDCLENKLUBU" NUMBER
   ) SEGMENT CREATION IMMEDIATE 
  PCTFREE 10 PCTUSED 40 INITRANS 1 MAXTRANS 255 
 NOCOMPRESS LOGGING
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "STUDENTI" ;

   COMMENT ON COLUMN "ST72870"."KONTRAKTY"."IDCLENKLUBU" IS 'Atribut idClenKlubu bude sloužit jako UID a bude nabývat èísel od 1 a výše';
   COMMENT ON TABLE "ST72870"."KONTRAKTY"  IS 'Tabulka KONTRAKTY slouží k uchování údajù o kontraktech.
Jsou zde øešena integritní omezení IO3, IO5, IO7.
Jsou zde øešena procedurální pravidla PP2, PP5, PP7.
Je zde øešeno strukturální pravidlo SP4';
--------------------------------------------------------
--  DDL for Table LOG_TABLE
--------------------------------------------------------

  CREATE TABLE "ST72870"."LOG_TABLE" 
   (	"IDLOG" NUMBER(*,0), 
	"OPERACE" VARCHAR2(30 BYTE), 
	"CAS" TIMESTAMP (6), 
	"UZIVATEL" VARCHAR2(30 BYTE), 
	"TABULKA" VARCHAR2(30 BYTE)
   ) SEGMENT CREATION IMMEDIATE 
  PCTFREE 10 PCTUSED 40 INITRANS 1 MAXTRANS 255 
 NOCOMPRESS LOGGING
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "STUDENTI" ;

   COMMENT ON COLUMN "ST72870"."LOG_TABLE"."IDLOG" IS 'Atribut idLog  bude sloužit jako UID a bude nabývat èísel od 1 výše';
   COMMENT ON TABLE "ST72870"."LOG_TABLE"  IS 'Tabulka LOG_TABLE slouží k logování zmìn dat v jednotlivých tabulkách v databázi';
--------------------------------------------------------
--  DDL for Table POZICE_HRAC
--------------------------------------------------------

  CREATE TABLE "ST72870"."POZICE_HRAC" 
   (	"ID_POZICE" NUMBER, 
	"NAZEV_POZICE" VARCHAR2(50 BYTE)
   ) SEGMENT CREATION IMMEDIATE 
  PCTFREE 10 PCTUSED 40 INITRANS 1 MAXTRANS 255 
 NOCOMPRESS LOGGING
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "STUDENTI" ;

   COMMENT ON COLUMN "ST72870"."POZICE_HRAC"."ID_POZICE" IS 'Atribut idPozice bude sloužit jako UID a bude nabývat èísel od 1 výše';
   COMMENT ON TABLE "ST72870"."POZICE_HRAC"  IS 'Tabulka POZICE_HRAC slouží jako èíselník pro pozice hráèù na høišti. Bude nabývat hodnot: BRANKÁØ, OBRÁNCE, ZÁLOŽNÍK, ÚTOÈNÍK. Je zde øešeno strukturální pravidlo SP6';
--------------------------------------------------------
--  DDL for Table ROLE
--------------------------------------------------------

  CREATE TABLE "ST72870"."ROLE" 
   (	"IDROLE" NUMBER(38,0) DEFAULT "ST72870"."SEKV_ROLE"."NEXTVAL", 
	"NAZEVROLE" VARCHAR2(8 BYTE)
   ) SEGMENT CREATION IMMEDIATE 
  PCTFREE 10 PCTUSED 40 INITRANS 1 MAXTRANS 255 
 NOCOMPRESS LOGGING
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "STUDENTI" ;

   COMMENT ON COLUMN "ST72870"."ROLE"."IDROLE" IS 'Atribut idRole bude sloužit jako UID a bude nabývat èísel od 1 výše';
   COMMENT ON TABLE "ST72870"."ROLE"  IS 'Tabulka ROLE slouží jako èíselník pro uživatelské role. Bude nabývat hodnot: ADMIN, UŽIVATEL, HRÁÈ, TRENÉR, HOST';
--------------------------------------------------------
--  DDL for Table SOUTEZE
--------------------------------------------------------

  CREATE TABLE "ST72870"."SOUTEZE" 
   (	"IDSOUTEZ" NUMBER(*,0), 
	"STARTDATUM" DATE, 
	"KONECDATUM" DATE, 
	"IDTYPSOUTEZE" NUMBER(*,0) DEFAULT 1
   ) SEGMENT CREATION IMMEDIATE 
  PCTFREE 10 PCTUSED 40 INITRANS 1 MAXTRANS 255 
 NOCOMPRESS LOGGING
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "STUDENTI" ;

   COMMENT ON COLUMN "ST72870"."SOUTEZE"."IDSOUTEZ" IS 'Atribut idSoutez bude sloužit jako UID a bude nabývat èísel od 1 výše.';
   COMMENT ON TABLE "ST72870"."SOUTEZE"  IS 'Tabulka SOUTEZE slouží k uchování údajù o soutìžích. Jsou zde øešena integritní omezení IO2, IO3';
--------------------------------------------------------
--  DDL for Table SPONZORI
--------------------------------------------------------

  CREATE TABLE "ST72870"."SPONZORI" 
   (	"IDSPONZOR" NUMBER(*,0), 
	"JMENO" VARCHAR2(30 BYTE), 
	"SPONZOROVANACASTKA" NUMBER(*,0)
   ) SEGMENT CREATION IMMEDIATE 
  PCTFREE 10 PCTUSED 40 INITRANS 1 MAXTRANS 255 
 NOCOMPRESS LOGGING
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "STUDENTI" ;

   COMMENT ON COLUMN "ST72870"."SPONZORI"."IDSPONZOR" IS 'Atribut idSponzor bude sloužit jako UID a bude nabývat èísel od 1 výše.';
   COMMENT ON TABLE "ST72870"."SPONZORI"  IS 'Tabulka SPONZORI, slouží k uchování ùdajù ohlednì jednotlivých sponzorù. Je zde øešeno integritní omezení IO7';
--------------------------------------------------------
--  DDL for Table SPONZORI_CLENOVE
--------------------------------------------------------

  CREATE TABLE "ST72870"."SPONZORI_CLENOVE" 
   (	"IDSPONZOR" NUMBER(*,0), 
	"IDCLENKLUBU" NUMBER(*,0)
   ) SEGMENT CREATION IMMEDIATE 
  PCTFREE 10 PCTUSED 40 INITRANS 1 MAXTRANS 255 
 NOCOMPRESS LOGGING
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "STUDENTI" ;

   COMMENT ON TABLE "ST72870"."SPONZORI_CLENOVE"  IS 'Tabulka SPONZORI_CLENOVE slouží  jako vazební tabulka mezi SPONZORI a CLENOVE_KLUBU. Je zde øešeno strukturální pravidlo SP7';
--------------------------------------------------------
--  DDL for Table SPONZORI_SOUTEZE
--------------------------------------------------------

  CREATE TABLE "ST72870"."SPONZORI_SOUTEZE" 
   (	"IDSPONZOR" NUMBER(*,0), 
	"IDSOUTEZ" NUMBER(*,0)
   ) SEGMENT CREATION IMMEDIATE 
  PCTFREE 10 PCTUSED 40 INITRANS 1 MAXTRANS 255 
 NOCOMPRESS LOGGING
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "STUDENTI" ;

   COMMENT ON TABLE "ST72870"."SPONZORI_SOUTEZE"  IS 'Tabulka SPONZORI_SOUTEZE  slouží  jako vazební tabulka mezi SPONZORI a SOUTEZE. Je zde øešeno strukturální pravidlo SP7';
--------------------------------------------------------
--  DDL for Table STAV_ZAPASU
--------------------------------------------------------

  CREATE TABLE "ST72870"."STAV_ZAPASU" 
   (	"IDSTAV" NUMBER(*,0), 
	"STAVZAPASU" VARCHAR2(12 CHAR)
   ) SEGMENT CREATION IMMEDIATE 
  PCTFREE 10 PCTUSED 40 INITRANS 1 MAXTRANS 255 
 NOCOMPRESS LOGGING
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "STUDENTI" ;

   COMMENT ON COLUMN "ST72870"."STAV_ZAPASU"."IDSTAV" IS 'Atribut idStav bude sloužit jako UID a bude nabývat èísel od 1 výše';
   COMMENT ON TABLE "ST72870"."STAV_ZAPASU"  IS 'Tabulka STAV_ZAPASU slouží jako èíselník pro definování stavu zápasu. Bude nabývat hodnot: ODEHRÁNO, BUDE SE HRÁT. Je zde øešeno strukturální pravidlo SP8';
--------------------------------------------------------
--  DDL for Table TRENERI
--------------------------------------------------------

  CREATE TABLE "ST72870"."TRENERI" 
   (	"IDCLENKLUBU" NUMBER(*,0), 
	"TRENERSKALICENCE" VARCHAR2(20 BYTE), 
	"SPECIALIZACE" VARCHAR2(30 BYTE), 
	"POCETLETPRAXE" NUMBER(*,0)
   ) SEGMENT CREATION IMMEDIATE 
  PCTFREE 10 PCTUSED 40 INITRANS 1 MAXTRANS 255 
 NOCOMPRESS LOGGING
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "STUDENTI" ;

   COMMENT ON COLUMN "ST72870"."TRENERI"."IDCLENKLUBU" IS 'Atribut idClenKlubu bude sloužit jako UID a bude nabývat èísel od 1 výše';
   COMMENT ON TABLE "ST72870"."TRENERI"  IS 'Tabulka TRENERI je subtypem CLENOVE_KLUBU, slouží k uchování  specifických  údajù pro trenéry. Jsou zde øešena integritní omezení IO1, IO7';
--------------------------------------------------------
--  DDL for Table TRENINKY
--------------------------------------------------------

  CREATE TABLE "ST72870"."TRENINKY" 
   (	"IDTRENINK" NUMBER(*,0), 
	"DATUM" DATE, 
	"MISTO" VARCHAR2(30 BYTE), 
	"POPIS" VARCHAR2(30 BYTE), 
	"IDCLENKLUBU" NUMBER(*,0)
   ) SEGMENT CREATION IMMEDIATE 
  PCTFREE 10 PCTUSED 40 INITRANS 1 MAXTRANS 255 
 NOCOMPRESS LOGGING
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "STUDENTI" ;

   COMMENT ON COLUMN "ST72870"."TRENINKY"."IDTRENINK" IS 'Atribut idTrenink bude sloužit jako UID a bude nabývat èísel od 1 výše.';
   COMMENT ON TABLE "ST72870"."TRENINKY"  IS 'Tabulka TRENINKY slouží k uchování  specifických údajù jednotlivých tréninkù.
Je zde øešeno integritní omezení IO4.
Je zde øešeno strukturální pravidlo SP2';
--------------------------------------------------------
--  DDL for Table TYP_SOUTEZ
--------------------------------------------------------

  CREATE TABLE "ST72870"."TYP_SOUTEZ" 
   (	"IDTYPSOUTEZE" NUMBER(*,0), 
	"NAZEVSOUTEZE" VARCHAR2(6 CHAR)
   ) SEGMENT CREATION IMMEDIATE 
  PCTFREE 10 PCTUSED 40 INITRANS 1 MAXTRANS 255 
 NOCOMPRESS LOGGING
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "STUDENTI" ;

   COMMENT ON COLUMN "ST72870"."TYP_SOUTEZ"."IDTYPSOUTEZE" IS 'Atribut idTypSouteze bude sloužit jako UID a bude nabývat èísel od 1 výše';
   COMMENT ON TABLE "ST72870"."TYP_SOUTEZ"  IS 'Tabulka TYP_SOUTEZ slouží jako èíselník pro definování konkrétní soutìže. Bude nabývat hodnot: LIGA, POHÁR, DIVIZE, KRAJ, OKRES';
--------------------------------------------------------
--  DDL for Table UZIVATELSKE_UCTY
--------------------------------------------------------

  CREATE TABLE "ST72870"."UZIVATELSKE_UCTY" 
   (	"IDUZIVATELSKYUCET" NUMBER(*,0), 
	"UZIVATELSKEJMENO" VARCHAR2(30 CHAR), 
	"HESLO" VARCHAR2(100 BYTE), 
	"POSLEDNIPRIHLASENI" DATE DEFAULT SYSTIMESTAMP, 
	"CLEN_KLUBU_IDCLENKLUBU" NUMBER(*,0), 
	"SALT" VARCHAR2(64 BYTE), 
	"EMAIL" VARCHAR2(100 BYTE), 
	"IDROLE" NUMBER(38,0)
   ) SEGMENT CREATION IMMEDIATE 
  PCTFREE 10 PCTUSED 40 INITRANS 1 MAXTRANS 255 
 NOCOMPRESS LOGGING
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "STUDENTI" ;

   COMMENT ON COLUMN "ST72870"."UZIVATELSKE_UCTY"."IDUZIVATELSKYUCET" IS 'Atribut idUzivatelskyUcet bude sloužit jako UID a bude nabývat èísel od 1 výše';
   COMMENT ON TABLE "ST72870"."UZIVATELSKE_UCTY"  IS 'Tabulka UZIVATELSKE_UCTY uchovává v sobì údaje o uživatelských úètech jednotlivých èlenù';
--------------------------------------------------------
--  DDL for Table VYSLEDKY_ZAPASU
--------------------------------------------------------

  CREATE TABLE "ST72870"."VYSLEDKY_ZAPASU" 
   (	"VYSLEDEK" VARCHAR2(5 BYTE), 
	"POCETZLUTYCHKARET" NUMBER(*,0), 
	"POCETCERVENYCHKARET" NUMBER(*,0), 
	"POCETGOLYDOMACITYM" NUMBER(*,0), 
	"POCETGOLYHOSTETYM" NUMBER(*,0), 
	"IDZAPAS" NUMBER(*,0)
   ) SEGMENT CREATION IMMEDIATE 
  PCTFREE 10 PCTUSED 40 INITRANS 1 MAXTRANS 255 
 NOCOMPRESS LOGGING
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "STUDENTI" ;

   COMMENT ON COLUMN "ST72870"."VYSLEDKY_ZAPASU"."IDZAPAS" IS 'Atribut idZapas bude sloužit jako UID a bude nabývat èísel od 1 výše';
   COMMENT ON TABLE "ST72870"."VYSLEDKY_ZAPASU"  IS 'Tabulka VYSLEDKY_ZAPASU slouží k uchování výsledkù odehraných zápasù.
Jsou zde øešena integritní omezení IO8, IO9.
Je zde øešeno strukturální pravidlo SP3.
Je zde øešeno procedurální pravidlo PP3';
--------------------------------------------------------
--  DDL for Table ZAPASY
--------------------------------------------------------

  CREATE TABLE "ST72870"."ZAPASY" 
   (	"IDZAPAS" NUMBER(*,0), 
	"DATUM" DATE, 
	"IDSOUTEZ" NUMBER(*,0), 
	"STAV_ZAPASU_IDSTAV" NUMBER DEFAULT 1, 
	"DOMACITYM" VARCHAR2(30 BYTE) DEFAULT 'SK Lhota II', 
	"HOSTETYM" VARCHAR2(30 BYTE) DEFAULT 'SK Lhota'
   ) SEGMENT CREATION IMMEDIATE 
  PCTFREE 10 PCTUSED 40 INITRANS 1 MAXTRANS 255 
 NOCOMPRESS LOGGING
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "STUDENTI" ;

   COMMENT ON COLUMN "ST72870"."ZAPASY"."IDZAPAS" IS 'Atribut idZapas bude sloužit jako UID a bude nabývat èísel od 1 výše';
   COMMENT ON TABLE "ST72870"."ZAPASY"  IS 'Tabulka ZAPASY slouží k uchování údajù o zápasech.
Jsou zde øešena strukturální pravidla SP1, SP5.
Je zde øešeno procedurální pravidlo PP6';
--------------------------------------------------------
--  DDL for View BINARNI_OBSAH_ROLE_VIEW
--------------------------------------------------------

  CREATE OR REPLACE FORCE EDITIONABLE VIEW "ST72870"."BINARNI_OBSAH_ROLE_VIEW" ("IDBINARNIOBSAH", "NAZEVSOUBORU", "TYPSOUBORU", "PRIPONASOUBORU", "OBSAH", "DATUMNAHRANI", "DATUMMODIFIKACE", "OPERACE", "IDUZIVATELSKYUCET", "ROLE") AS 
  SELECT 
    b.IDBINARNIOBSAH,
    b.NAZEVSOUBORU,
    b.TYPSOUBORU,
    b.PRIPONASOUBORU,
    b.OBSAH,
    b.DATUMNAHRANI,
    b.DATUMMODIFIKACE,
    b.OPERACE,
    b.IDUZIVATELSKYUCET,
    r.NAZEVROLE AS ROLE
FROM BINARNI_OBSAH b
LEFT JOIN UZIVATELSKE_UCTY u 
    ON b.IDUZIVATELSKYUCET = u.IDUZIVATELSKYUCET
LEFT JOIN ROLE r 
    ON u.IDROLE = r.IDROLE;

   COMMENT ON TABLE "ST72870"."BINARNI_OBSAH_ROLE_VIEW"  IS 'Pohled BINARNI_OBSAH_ROLE_VIEW slouží k propojení informací o binárním obsahu s údaji o uživateli, který obsah nahrál a jeho roli'
;
--------------------------------------------------------
--  DDL for View CASTKY_SPONZOROVANYCH_SOUTEZI_VIEW
--------------------------------------------------------

  CREATE OR REPLACE FORCE EDITIONABLE VIEW "ST72870"."CASTKY_SPONZOROVANYCH_SOUTEZI_VIEW" ("IDSOUTEZ", "NAZEVSOUTEZE", "CELKOVACASTKA") AS 
  SELECT
    sout.idSoutez,
    typsout.nazevSouteze,
    SUM(spon.sponzorovanaCastka) AS celkovaCastka
FROM souteze sout
JOIN sponzori_souteze sponsout ON sout.idsoutez = sponsout.idsoutez
JOIN sponzori spon ON sponsout.idsponzor = spon.idsponzor
JOIN typ_soutez typsout ON sout.idtypsouteze = typsout.idtypsouteze
GROUP BY sout.idsoutez, typsout.nazevsouteze;

   COMMENT ON TABLE "ST72870"."CASTKY_SPONZOROVANYCH_SOUTEZI_VIEW"  IS 'Pohled CASTKY_SPONZOROVANYCH_SOUTEZI_VIEW slouží k zobrazení celkového poètu sponzorovaných èástek pro každou soutìž'
;
--------------------------------------------------------
--  DDL for View CLENOVE_KLUBU_VIEW
--------------------------------------------------------

  CREATE OR REPLACE FORCE EDITIONABLE VIEW "ST72870"."CLENOVE_KLUBU_VIEW" ("IDCLENKLUBU", "JMENO", "PRIJMENI", "TYPCLENA", "TELEFONNICISLO", "RODNE_CISLO") AS 
  SELECT "IDCLENKLUBU","JMENO","PRIJMENI","TYPCLENA","TELEFONNICISLO","RODNE_CISLO" FROM clenove_klubu;

   COMMENT ON TABLE "ST72870"."CLENOVE_KLUBU_VIEW"  IS 'Pohled CLENOVE_KLUBU_VIEW slouží k zobrazení jednotlivých hráèù a jejich údajù'
;
--------------------------------------------------------
--  DDL for View HRACI_OPATRENI_VIEW
--------------------------------------------------------

  CREATE OR REPLACE FORCE EDITIONABLE VIEW "ST72870"."HRACI_OPATRENI_VIEW" ("IDCLENKLUBU", "RODNE_CISLO", "JMENO", "PRIJMENI", "TYPCLENA", "TELEFONNICISLO", "POCETVSTRELENYCHGOLU", "POCET_ZLUTYCH_KARET", "POCET_CERVENYCH_KARET", "POZICENAHRISTI", "DATUMOPATRENI", "DELKATRESTU", "DUVOD") AS 
  SELECT
    h.IDCLENKLUBU,
    c.RODNE_CISLO,
    c.JMENO,
    c.PRIJMENI,
    c.TYPCLENA,
    c.TELEFONNICISLO,
    h.POCETVSTRELENYCHGOLU,
    h.POCET_ZLUTYCH_KARET,
    h.POCET_CERVENYCH_KARET,
    p.NAZEV_POZICE AS POZICENAHRISTI,
    d.DATUMOPATRENI,
    d.DELKATRESTU,
    d.DUVOD
FROM HRACI h
JOIN CLENOVE_KLUBU c 
    ON h.IDCLENKLUBU = c.IDCLENKLUBU
LEFT JOIN DISCIPLINARNI_OPATRENI d 
    ON h.IDOPATRENI = d.IDOPATRENI
JOIN POZICE_HRAC p      
    ON h.ID_POZICE = p.ID_POZICE;

   COMMENT ON TABLE "ST72870"."HRACI_OPATRENI_VIEW"  IS 'Pohled HRACI_OPATRENI_VIEW slouží k zobrazení jednotlivých disciplinárních opatøení, která jsou pøiøazena hráèùm a také zobrazí osobní údaje hráèù'
;
--------------------------------------------------------
--  DDL for View KONTRAKTY_VIEW
--------------------------------------------------------

  CREATE OR REPLACE FORCE EDITIONABLE VIEW "ST72870"."KONTRAKTY_VIEW" ("IDCLENKLUBU", "DATUMZACATKU", "DATUMKONCE", "PLAT", "CISLONAAGENTA", "VYSTUPNIKLAUZULE", "JMENO", "PRIJMENI", "RODNE_CISLO") AS 
  SELECT  
    kon.idclenklubu,
    kon.datumzacatku, 
    kon.datumkonce, 
    kon.plat, 
    kon.cislonaagenta, 
    kon.vystupniklauzule,
    clen.jmeno,
    clen.prijmeni,
    clen.rodne_cislo
FROM kontrakty kon
LEFT JOIN clenove_klubu clen ON kon.idclenklubu = clen.idclenklubu;

   COMMENT ON TABLE "ST72870"."KONTRAKTY_VIEW"  IS 'Pohled KONTRAKY_VIEW slouží k zobrazení jednotlivých hráèských kontraktù a také zobrazí jméno, pøíjmení a rodné èíslo hráèe'
;
--------------------------------------------------------
--  DDL for View LOG_TABLE_VIEW
--------------------------------------------------------

  CREATE OR REPLACE FORCE EDITIONABLE VIEW "ST72870"."LOG_TABLE_VIEW" ("IDLOG", "OPERACE", "CAS", "UZIVATEL", "TABULKA") AS 
  SELECT "IDLOG","OPERACE","CAS","UZIVATEL","TABULKA" FROM log_table;

   COMMENT ON TABLE "ST72870"."LOG_TABLE_VIEW"  IS 'Pohled LOG_TABLE_VIEW slouží k zobrazení údajù ohlednì všech zaznamenaných zmìn v tabulkách'
;
--------------------------------------------------------
--  DDL for View PREHLED_UCTY_ROLE_VIEW
--------------------------------------------------------

  CREATE OR REPLACE FORCE EDITIONABLE VIEW "ST72870"."PREHLED_UCTY_ROLE_VIEW" ("IDUZIVATELSKYUCET", "IDROLE", "ROLE") AS 
  SELECT 
    u.IDUZIVATELSKYUCET,
    u.IDROLE,
    r.NAZEVROLE AS ROLE
FROM UZIVATELSKE_UCTY u
JOIN ROLE r ON u.IDROLE = r.IDROLE;

   COMMENT ON TABLE "ST72870"."PREHLED_UCTY_ROLE_VIEW"  IS 'Pohled PREHLED_UCTY_ROLE_VIEW slouží k zobrazení jaké uživatelské úèty mají jaké role'
;
--------------------------------------------------------
--  DDL for View PREHLED_UZIVATELSKE_UCTY
--------------------------------------------------------

  CREATE OR REPLACE FORCE EDITIONABLE VIEW "ST72870"."PREHLED_UZIVATELSKE_UCTY" ("IDUZIVATELSKYUCET", "UZIVATELSKEJMENO", "HESLO", "SALT", "ROLE", "EMAIL", "POSLEDNIPRIHLASENI", "IDCLENKLUBU", "RODNE_CISLO", "JMENO", "PRIJMENI", "TYPCLENA", "TELEFONNICISLO") AS 
  SELECT
    u.IDUZIVATELSKYUCET,
    u.UZIVATELSKEJMENO,
    u.HESLO,
    u.SALT,
    r.NAZEVROLE AS ROLE,
    u.EMAIL,
    u.POSLEDNIPRIHLASENI,
    c.IDCLENKLUBU,
    c.RODNE_CISLO,
    c.JMENO,
    c.PRIJMENI,
    c.TYPCLENA,
    c.TELEFONNICISLO
FROM 
    UZIVATELSKE_UCTY u
    JOIN ROLE r ON u.IDROLE = r.IDROLE
    LEFT JOIN CLENOVE_KLUBU c ON c.IDCLENKLUBU = u.CLEN_KLUBU_IDCLENKLUBU;

   COMMENT ON TABLE "ST72870"."PREHLED_UZIVATELSKE_UCTY"  IS 'Pohled PREHLED_UZIVATELSKE_UCTY slouží k zobrazení údajù jednotlivých uživatelských úètù a také zobrazí základní údaje ohlednì èlena klubu, který je spjatý s daným úètem'
;
--------------------------------------------------------
--  DDL for View ROLE_VIEW
--------------------------------------------------------

  CREATE OR REPLACE FORCE EDITIONABLE VIEW "ST72870"."ROLE_VIEW" ("IDROLE", "NAZEVROLE") AS 
  SELECT IDROLE, NAZEVROLE
FROM ROLE;

   COMMENT ON TABLE "ST72870"."ROLE_VIEW"  IS 'Pohled ROLE_VIEW slouží k zobrazení jednotlivých rolí'
;
--------------------------------------------------------
--  DDL for View SOUTEZE_VIEW
--------------------------------------------------------

  CREATE OR REPLACE FORCE EDITIONABLE VIEW "ST72870"."SOUTEZE_VIEW" ("IDSOUTEZ", "STARTDATUM", "KONECDATUM", "IDTYPSOUTEZE", "NAZEVSOUTEZE") AS 
  SELECT sout.idsoutez, sout.startdatum, sout.konecdatum, typsout.idtypsouteze, typsout.nazevsouteze
FROM souteze sout
JOIN typ_soutez typsout ON sout.idtypsouteze = typsout.idtypsouteze;

   COMMENT ON TABLE "ST72870"."SOUTEZE_VIEW"  IS 'Pohled SOUTEZE_VIEW slouží k zobrazení údajù o všech soutìží'
;
--------------------------------------------------------
--  DDL for View SPONZORI_VIEW
--------------------------------------------------------

  CREATE OR REPLACE FORCE EDITIONABLE VIEW "ST72870"."SPONZORI_VIEW" ("IDSPONZOR", "JMENO_SPONZORA", "SPONZOROVANACASTKA", "IDCLENKLUBU", "JMENO_CLENA", "PRIJMENI_CLENA", "RODNE_CISLO", "IDSOUTEZ", "STARTDATUM", "KONECDATUM", "NAZEVSOUTEZE") AS 
  SELECT 
    spon.idsponzor,
    spon.jmeno AS JMENO_SPONZORA,
    spon.sponzorovanacastka,
    clen.idclenklubu,
    clen.jmeno AS JMENO_CLENA,
    clen.prijmeni AS PRIJMENI_CLENA,
    clen.rodne_cislo,
    sout.idsoutez,
    sout.startdatum,
    sout.konecdatum,
    typsout.nazevsouteze
FROM sponzori spon
LEFT JOIN sponzori_clenove sponclen 
    ON spon.idsponzor = sponclen.idsponzor
LEFT JOIN clenove_klubu clen 
    ON clen.idclenklubu = sponclen.idclenklubu
LEFT JOIN sponzori_souteze sponsoutez 
    ON spon.idsponzor = sponsoutez.idsponzor
LEFT JOIN souteze sout 
    ON sout.idsoutez = sponsoutez.idsoutez
LEFT JOIN typ_soutez typsout
    ON sout.idtypsouteze = typsout.idtypsouteze;

   COMMENT ON TABLE "ST72870"."SPONZORI_VIEW"  IS 'Pohled SPONZORI_VIEW slouží k zobrazení údajù o všech sponzorech a také zobrazí základní údaje o hráèích a soutìžích, které jsou sponzorované daným sponzorem'
;
--------------------------------------------------------
--  DDL for View STAV_ZAPASU_VIEW
--------------------------------------------------------

  CREATE OR REPLACE FORCE EDITIONABLE VIEW "ST72870"."STAV_ZAPASU_VIEW" ("IDSTAV", "STAVZAPASU") AS 
  SELECT idstav, stav_zapasu.stavzapasu
FROM stav_zapasu;

   COMMENT ON TABLE "ST72870"."STAV_ZAPASU_VIEW"  IS 'Pohled STAV_ZAPASU_VIEW slouží k zobrazení všech stavù zápasu '
;
--------------------------------------------------------
--  DDL for View TABULKY_VIEW
--------------------------------------------------------

  CREATE OR REPLACE FORCE EDITIONABLE VIEW "ST72870"."TABULKY_VIEW" ("TABULKA") AS 
  SELECT table_name AS tabulka FROM user_tables;

   COMMENT ON TABLE "ST72870"."TABULKY_VIEW"  IS 'Pohled TABULKY_VIEW slouží k zobrazení názvù tabulek, které byly vytvoøeny v databázovém schématu'
;
--------------------------------------------------------
--  DDL for View TOP_3_NEJLEPSI_STRELCI_VIEW
--------------------------------------------------------

  CREATE OR REPLACE FORCE EDITIONABLE VIEW "ST72870"."TOP_3_NEJLEPSI_STRELCI_VIEW" ("IDCLENKLUBU", "JMENO", "PRIJMENI", "POCETVSTRELENYCHGOLU", "NAZEV_POZICE", "PORADI") AS 
  SELECT 
    idclenklubu,
    jmeno,
    prijmeni,
    pocetvstrelenychgolu,
    nazev_pozice,
    poradi
FROM
(
    SELECT
        hr.idclenklubu,
        clen.jmeno,
        clen.prijmeni,
        hr.pocetvstrelenychgolu,
        poz.nazev_pozice,
        ROW_NUMBER() OVER (ORDER BY hr.pocetvstrelenychgolu DESC) AS poradi
    FROM hraci hr
    JOIN clenove_klubu clen ON hr.idclenklubu = clen.idclenklubu
    JOIN pozice_hrac poz ON poz.id_pozice = hr.id_pozice
)
WHERE poradi <= 3;

   COMMENT ON TABLE "ST72870"."TOP_3_NEJLEPSI_STRELCI_VIEW"  IS 'Pohled TOP_3_NEJLEPSI_STRELCI_VIEW slouží k zobrazení TOP 3 nejlepších støelcù, zobrazí se jejich jména, pøíjmení, poèet gólù, pozice na høištì a jejich poøadí'
;
--------------------------------------------------------
--  DDL for View TRENERI_VIEW
--------------------------------------------------------

  CREATE OR REPLACE FORCE EDITIONABLE VIEW "ST72870"."TRENERI_VIEW" ("IDCLENKLUBU", "RODNE_CISLO", "JMENO", "PRIJMENI", "TYPCLENA", "TELEFONNICISLO", "TRENERSKALICENCE", "SPECIALIZACE", "POCETLETPRAXE") AS 
  SELECT c.IDCLENKLUBU,
       c.RODNE_CISLO,
       c.JMENO,
       c.PRIJMENI,
       c.TYPCLENA,
       c.TELEFONNICISLO,
       t.TRENERSKALICENCE,
       t.SPECIALIZACE,
       t.POCETLETPRAXE
FROM CLENOVE_KLUBU c
INNER JOIN TRENERI t ON c.IDCLENKLUBU = t.IDCLENKLUBU;

   COMMENT ON TABLE "ST72870"."TRENERI_VIEW"  IS 'Pohled TRENERI_VIEW slouží k zobrazení údajù o všech trenérech'
;
--------------------------------------------------------
--  DDL for View TRENINKY_VIEW
--------------------------------------------------------

  CREATE OR REPLACE FORCE EDITIONABLE VIEW "ST72870"."TRENINKY_VIEW" ("IDTRENINK", "DATUM", "MISTO", "POPIS", "IDCLENKLUBU", "PRIJMENI", "RODNE_CISLO") AS 
  SELECT 
    t.IDTRENINK,
    t.DATUM,
    t.MISTO,
    t.POPIS,
    t.IDCLENKLUBU,
    c.PRIJMENI,
    c.RODNE_CISLO
FROM TRENINKY t
JOIN CLENOVE_KLUBU c
    ON t.IDCLENKLUBU = c.IDCLENKLUBU
WHERE c.TYPCLENA = 'Trener';

   COMMENT ON TABLE "ST72870"."TRENINKY_VIEW"  IS 'Pohled TRENINKY_VIEW slouží k zobrazení údajù o tréninku a zobrazí se také jméno a pøíjmení trenéra, který daný trénink vedl'
;
--------------------------------------------------------
--  DDL for View TYP_SOUTEZ_VIEW
--------------------------------------------------------

  CREATE OR REPLACE FORCE EDITIONABLE VIEW "ST72870"."TYP_SOUTEZ_VIEW" ("IDTYPSOUTEZE", "NAZEVSOUTEZE") AS 
  SELECT idtypsouteze, nazevsouteze
FROM typ_soutez;

   COMMENT ON TABLE "ST72870"."TYP_SOUTEZ_VIEW"  IS 'Pohled TYP_SOUTEZ_VIEW slouží k zobrazení typù soutìží'
;
--------------------------------------------------------
--  DDL for View VYSLEDKY_ZAPASU_VIEW
--------------------------------------------------------

  CREATE OR REPLACE FORCE EDITIONABLE VIEW "ST72870"."VYSLEDKY_ZAPASU_VIEW" ("DOMACITYM", "HOSTETYM", "IDZAPAS", "POCETZLUTYCHKARET", "POCETCERVENYCHKARET", "POCETGOLYDOMACITYM", "POCETGOLYHOSTETYM", "VYSLEDEK") AS 
  SELECT zap.domacitym, zap.hostetym,
       vys.idzapas, vys.pocetzlutychkaret, vys.pocetcervenychkaret, 
       vys.pocetgolydomacitym, vys.pocetgolyhostetym, vys.vysledek
FROM vysledky_zapasu vys
JOIN zapasy zap ON vys.idzapas = zap.idzapas;

   COMMENT ON TABLE "ST72870"."VYSLEDKY_ZAPASU_VIEW"  IS 'Pohled VYSLEDKY_ZAPASU_VIEW slouží k zobrazení výsledkù všech zápasù a zobrazí také domácí a hostující tým daného zápasu'
;
--------------------------------------------------------
--  DDL for View ZAPASY_VIEW
--------------------------------------------------------

  CREATE OR REPLACE FORCE EDITIONABLE VIEW "ST72870"."ZAPASY_VIEW" ("IDZAPAS", "DATUM", "DOMACITYM", "HOSTETYM", "VYSLEDEK", "IDSOUTEZ", "NAZEVSOUTEZE", "STAV_ZAPASU_IDSTAV", "STAVZAPASU") AS 
  SELECT zap.idzapas, zap.datum, zap.domacitym, zap.hostetym, vys.vysledek, zap.idsoutez, 
        typsout.nazevsouteze, zap.stav_zapasu_idstav, stavzap.stavzapasu      
FROM zapasy zap
JOIN typ_soutez typsout ON typsout.idtypsouteze = zap.idsoutez
JOIN stav_zapasu stavzap ON stavzap.idstav = zap.stav_zapasu_idstav
LEFT JOIN vysledky_zapasu vys ON vys.idzapas = zap.idzapas;

   COMMENT ON TABLE "ST72870"."ZAPASY_VIEW"  IS 'Pohled ZAPASY_VIEW slouží k zobrazení údajù o zápasech a zobrazí také výsledek, aktuální stav zápasu a základní údaje o soutìži, ve které je daný zápas evidován'
;
--------------------------------------------------------
--  DDL for Index SPONZORI_CLENOVE_PK
--------------------------------------------------------

  CREATE UNIQUE INDEX "ST72870"."SPONZORI_CLENOVE_PK" ON "ST72870"."SPONZORI_CLENOVE" ("IDSPONZOR", "IDCLENKLUBU") 
  PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "STUDENTI" ;
--------------------------------------------------------
--  DDL for Index SPONZORI_JMENO_UN
--------------------------------------------------------

  CREATE UNIQUE INDEX "ST72870"."SPONZORI_JMENO_UN" ON "ST72870"."SPONZORI" ("JMENO") 
  PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "STUDENTI" ;
--------------------------------------------------------
--  DDL for Index SPONZORI_PK
--------------------------------------------------------

  CREATE UNIQUE INDEX "ST72870"."SPONZORI_PK" ON "ST72870"."SPONZORI" ("IDSPONZOR") 
  PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "STUDENTI" ;
--------------------------------------------------------
--  DDL for Index SPONZORI_SOUTEZE_PK
--------------------------------------------------------

  CREATE UNIQUE INDEX "ST72870"."SPONZORI_SOUTEZE_PK" ON "ST72870"."SPONZORI_SOUTEZE" ("IDSPONZOR", "IDSOUTEZ") 
  PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "STUDENTI" ;
--------------------------------------------------------
--  DDL for Index STAV_ZAPASU_PK
--------------------------------------------------------

  CREATE UNIQUE INDEX "ST72870"."STAV_ZAPASU_PK" ON "ST72870"."STAV_ZAPASU" ("IDSTAV") 
  PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "STUDENTI" ;
--------------------------------------------------------
--  DDL for Index STAV_ZAPASU_STAVZAPASU_UN
--------------------------------------------------------

  CREATE UNIQUE INDEX "ST72870"."STAV_ZAPASU_STAVZAPASU_UN" ON "ST72870"."STAV_ZAPASU" ("STAVZAPASU") 
  PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "STUDENTI" ;
--------------------------------------------------------
--  DDL for Index TRENERI_PK
--------------------------------------------------------

  CREATE UNIQUE INDEX "ST72870"."TRENERI_PK" ON "ST72870"."TRENERI" ("IDCLENKLUBU") 
  PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "STUDENTI" ;
--------------------------------------------------------
--  DDL for Index TRENINKY_PK
--------------------------------------------------------

  CREATE UNIQUE INDEX "ST72870"."TRENINKY_PK" ON "ST72870"."TRENINKY" ("IDTRENINK") 
  PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "STUDENTI" ;
--------------------------------------------------------
--  DDL for Index TYP_SOUTEZ_NAZEVSOUTEZE_UN
--------------------------------------------------------

  CREATE UNIQUE INDEX "ST72870"."TYP_SOUTEZ_NAZEVSOUTEZE_UN" ON "ST72870"."TYP_SOUTEZ" ("NAZEVSOUTEZE") 
  PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "STUDENTI" ;
--------------------------------------------------------
--  DDL for Index TYP_SOUTEZ_PK
--------------------------------------------------------

  CREATE UNIQUE INDEX "ST72870"."TYP_SOUTEZ_PK" ON "ST72870"."TYP_SOUTEZ" ("IDTYPSOUTEZE") 
  PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "STUDENTI" ;
--------------------------------------------------------
--  DDL for Index UZIVATELSKY_UCET_PK
--------------------------------------------------------

  CREATE UNIQUE INDEX "ST72870"."UZIVATELSKY_UCET_PK" ON "ST72870"."UZIVATELSKE_UCTY" ("IDUZIVATELSKYUCET") 
  PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "STUDENTI" ;
--------------------------------------------------------
--  DDL for Index UZIVATEL_EMAIL_UN
--------------------------------------------------------

  CREATE UNIQUE INDEX "ST72870"."UZIVATEL_EMAIL_UN" ON "ST72870"."UZIVATELSKE_UCTY" ("EMAIL") 
  PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "STUDENTI" ;
--------------------------------------------------------
--  DDL for Index UZIVATEL_JMENO_UN
--------------------------------------------------------

  CREATE UNIQUE INDEX "ST72870"."UZIVATEL_JMENO_UN" ON "ST72870"."UZIVATELSKE_UCTY" ("UZIVATELSKEJMENO") 
  PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "STUDENTI" ;
--------------------------------------------------------
--  DDL for Index VYSLEDKY_ZAPASU_PK
--------------------------------------------------------

  CREATE UNIQUE INDEX "ST72870"."VYSLEDKY_ZAPASU_PK" ON "ST72870"."VYSLEDKY_ZAPASU" ("IDZAPAS") 
  PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "STUDENTI" ;
--------------------------------------------------------
--  DDL for Index ZAPASY_PK
--------------------------------------------------------

  CREATE UNIQUE INDEX "ST72870"."ZAPASY_PK" ON "ST72870"."ZAPASY" ("IDZAPAS") 
  PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "STUDENTI" ;
--------------------------------------------------------
--  DDL for Index HRACI_PK
--------------------------------------------------------

  CREATE UNIQUE INDEX "ST72870"."HRACI_PK" ON "ST72870"."HRACI" ("IDCLENKLUBU") 
  PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "STUDENTI" ;
--------------------------------------------------------
--  DDL for Index KONTRAKTY_PK
--------------------------------------------------------

  CREATE UNIQUE INDEX "ST72870"."KONTRAKTY_PK" ON "ST72870"."KONTRAKTY" ("IDCLENKLUBU") 
  PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "STUDENTI" ;
--------------------------------------------------------
--  DDL for Index LOG_TABLE_PK
--------------------------------------------------------

  CREATE UNIQUE INDEX "ST72870"."LOG_TABLE_PK" ON "ST72870"."LOG_TABLE" ("IDLOG") 
  PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "STUDENTI" ;
--------------------------------------------------------
--  DDL for Index POZICE_HRAC_NAZEVPOZICE_UN
--------------------------------------------------------

  CREATE UNIQUE INDEX "ST72870"."POZICE_HRAC_NAZEVPOZICE_UN" ON "ST72870"."POZICE_HRAC" ("NAZEV_POZICE") 
  PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "STUDENTI" ;
--------------------------------------------------------
--  DDL for Index POZICE_HRAC_PK
--------------------------------------------------------

  CREATE UNIQUE INDEX "ST72870"."POZICE_HRAC_PK" ON "ST72870"."POZICE_HRAC" ("ID_POZICE") 
  PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "STUDENTI" ;
--------------------------------------------------------
--  DDL for Index ROLE_NAZEVROLE_UN
--------------------------------------------------------

  CREATE UNIQUE INDEX "ST72870"."ROLE_NAZEVROLE_UN" ON "ST72870"."ROLE" ("NAZEVROLE") 
  PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "STUDENTI" ;
--------------------------------------------------------
--  DDL for Index ROLE_PK
--------------------------------------------------------

  CREATE UNIQUE INDEX "ST72870"."ROLE_PK" ON "ST72870"."ROLE" ("IDROLE") 
  PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "STUDENTI" ;
--------------------------------------------------------
--  DDL for Index SOUTEZE_PK
--------------------------------------------------------

  CREATE UNIQUE INDEX "ST72870"."SOUTEZE_PK" ON "ST72870"."SOUTEZE" ("IDSOUTEZ") 
  PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "STUDENTI" ;
--------------------------------------------------------
--  DDL for Index BINARNI_OBSAH_PK
--------------------------------------------------------

  CREATE UNIQUE INDEX "ST72870"."BINARNI_OBSAH_PK" ON "ST72870"."BINARNI_OBSAH" ("IDBINARNIOBSAH") 
  PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "STUDENTI" ;
--------------------------------------------------------
--  DDL for Index CLENOVE_KLUBU_PK
--------------------------------------------------------

  CREATE UNIQUE INDEX "ST72870"."CLENOVE_KLUBU_PK" ON "ST72870"."CLENOVE_KLUBU" ("IDCLENKLUBU") 
  PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "STUDENTI" ;
--------------------------------------------------------
--  DDL for Index CLEN_KLUBU_RODNECISLO_UN
--------------------------------------------------------

  CREATE UNIQUE INDEX "ST72870"."CLEN_KLUBU_RODNECISLO_UN" ON "ST72870"."CLENOVE_KLUBU" ("RODNE_CISLO") 
  PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "STUDENTI" ;
--------------------------------------------------------
--  DDL for Index DISCIPLINARNI_OPATRENI_PK
--------------------------------------------------------

  CREATE UNIQUE INDEX "ST72870"."DISCIPLINARNI_OPATRENI_PK" ON "ST72870"."DISCIPLINARNI_OPATRENI" ("IDOPATRENI") 
  PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "STUDENTI" ;
--------------------------------------------------------
--  DDL for Index UZIVATEL_UCET_IDX
--------------------------------------------------------

  CREATE UNIQUE INDEX "ST72870"."UZIVATEL_UCET_IDX" ON "ST72870"."UZIVATELSKE_UCTY" ("CLEN_KLUBU_IDCLENKLUBU") 
  PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "STUDENTI" ;
--------------------------------------------------------
--  DDL for Trigger ARC_ARC_CLENOVEKLUBU_HRACI
--------------------------------------------------------

  CREATE OR REPLACE EDITIONABLE TRIGGER "ST72870"."ARC_ARC_CLENOVEKLUBU_HRACI" BEFORE
    INSERT OR UPDATE OF idclenklubu ON hraci
    FOR EACH ROW
DECLARE
    d VARCHAR2(20);
BEGIN
    SELECT
        a.typclena
    INTO d
    FROM
        clenove_klubu a
    WHERE
        a.idclenklubu = :new.idclenklubu;

    IF ( d IS NULL OR d <> 'Hrac' ) THEN
        raise_application_error(-20223, 'FK HRACI_CLENOVE_KLUBU_FK in Table HRACI violates Arc constraint on Table CLENOVE_KLUBU - discriminator column typClena doesn''t have value ''Hrac'''
        );
    END IF;

EXCEPTION
    WHEN no_data_found THEN
        NULL;
    WHEN OTHERS THEN
        RAISE;
END;

/
ALTER TRIGGER "ST72870"."ARC_ARC_CLENOVEKLUBU_HRACI" ENABLE;
--------------------------------------------------------
--  DDL for Trigger ARC_ARC_CLENOVEKLUBU_TRENERI
--------------------------------------------------------

  CREATE OR REPLACE EDITIONABLE TRIGGER "ST72870"."ARC_ARC_CLENOVEKLUBU_TRENERI" BEFORE
    INSERT OR UPDATE OF idclenklubu ON treneri
    FOR EACH ROW
DECLARE
    d VARCHAR2(20);
BEGIN
    SELECT
        a.typclena
    INTO d
    FROM
        clenove_klubu a
    WHERE
        a.idclenklubu = :new.idclenklubu;

    IF ( d IS NULL OR d <> 'Trener' ) THEN
        raise_application_error(-20223, 'FK TRENERI_CLENOVE_KLUBU_FK in Table TRENERI violates Arc constraint on Table CLENOVE_KLUBU - discriminator column typClena doesn''t have value ''Trener'''
        );
    END IF;

EXCEPTION
    WHEN no_data_found THEN
        NULL;
    WHEN OTHERS THEN
        RAISE;
END;

/
ALTER TRIGGER "ST72870"."ARC_ARC_CLENOVEKLUBU_TRENERI" ENABLE;
--------------------------------------------------------
--  DDL for Trigger TRIG_KONTROLA_DATUMU_KONTRAKTY
--------------------------------------------------------

  CREATE OR REPLACE EDITIONABLE TRIGGER "ST72870"."TRIG_KONTROLA_DATUMU_KONTRAKTY" 
BEFORE INSERT OR UPDATE ON KONTRAKTY
FOR EACH ROW
BEGIN
    pkg_kontrakty.sp_kontrola_datumu(:new.datumzacatku, :new.datumkonce);
END;
/
ALTER TRIGGER "ST72870"."TRIG_KONTROLA_DATUMU_KONTRAKTY" ENABLE;
--------------------------------------------------------
--  DDL for Trigger TRIG_KONTROLA_DATUMU_SOUTEZE
--------------------------------------------------------

  CREATE OR REPLACE EDITIONABLE TRIGGER "ST72870"."TRIG_KONTROLA_DATUMU_SOUTEZE" 
BEFORE INSERT OR UPDATE ON SOUTEZE 
FOR EACH ROW
BEGIN
    pkg_souteze.sp_kontrola_datumu(:new.startdatum, :new.konecdatum);
END;
/
ALTER TRIGGER "ST72870"."TRIG_KONTROLA_DATUMU_SOUTEZE" ENABLE;
--------------------------------------------------------
--  DDL for Trigger TRIG_KONTROLA_PLATNEHO_KONTRAKTU
--------------------------------------------------------

  CREATE OR REPLACE EDITIONABLE TRIGGER "ST72870"."TRIG_KONTROLA_PLATNEHO_KONTRAKTU" 
BEFORE INSERT OR UPDATE ON kontrakty
FOR EACH ROW
BEGIN

  -- Kontrola, že kontrakt bude trvat pøesnì 1 rok (12 mìsícù)
  IF TRUNC(:new.datumkonce) <> ADD_MONTHS(TRUNC(:new.DATUMZACATKU), 12) THEN
    RAISE_APPLICATION_ERROR(-20001, 'Kontrakt musí mít platnost pøesnì 1 rok (od data zaèátku + 12 mìsícù)!');
  END IF;

END;
/
ALTER TRIGGER "ST72870"."TRIG_KONTROLA_PLATNEHO_KONTRAKTU" ENABLE;
--------------------------------------------------------
--  DDL for Trigger TRIG_KONTROLA_TYMU_ZAPASY
--------------------------------------------------------

  CREATE OR REPLACE EDITIONABLE TRIGGER "ST72870"."TRIG_KONTROLA_TYMU_ZAPASY" 
BEFORE INSERT OR UPDATE OF domacitym, hostetym
ON zapasy
FOR EACH ROW
BEGIN

    -- Kontrola, zda domácí tým nehraje sám se sebou
    IF LOWER(:NEW.domacitym) = LOWER(:NEW.hostetym) THEN
        RAISE_APPLICATION_ERROR(-20001, 'Domácí tým nemùže hrát sám se sebou!');
    END IF;

END;
/
ALTER TRIGGER "ST72870"."TRIG_KONTROLA_TYMU_ZAPASY" ENABLE;
--------------------------------------------------------
--  DDL for Trigger TRIG_KONTROLA_VYSLEDKU_VYSLEDKY_ZAPASU
--------------------------------------------------------

  CREATE OR REPLACE EDITIONABLE TRIGGER "ST72870"."TRIG_KONTROLA_VYSLEDKU_VYSLEDKY_ZAPASU" 
BEFORE INSERT OR UPDATE OF VYSLEDEK, POCETGOLYDOMACITYM, POCETGOLYHOSTETYM
ON VYSLEDKY_ZAPASU 
FOR EACH ROW
DECLARE

    v_goly_domaci NUMBER;
    v_goly_hoste  NUMBER;
    
BEGIN

    -- Pokud není vyplnìný výsledek, nic se nekontroluje
    IF :NEW.VYSLEDEK IS NULL THEN
        RETURN;
    END IF;

    -- Pokud formát nového výsledku není validní, vyhodí se vlastní vyjímka -20002
    IF pkg_vysledky_zapasu.fn_je_format_vysledku_validni(:NEW.vysledek) = 0 THEN
        RAISE_APPLICATION_ERROR(-20002, 'Neplatný výsledek zápasu. Formát musí být "DOMÁCÍ:HOSTÉ" a mùže mít maximálnì 5 znakù!');
    END IF;

    -- Rozparsování výsledku na domácí a hosty
    v_goly_domaci := TO_NUMBER(SUBSTR(:NEW.VYSLEDEK, 1, INSTR(:NEW.VYSLEDEK, ':') - 1));
    v_goly_hoste  := TO_NUMBER(SUBSTR(:NEW.VYSLEDEK, INSTR(:NEW.VYSLEDEK, ':') + 1));

    -- Pokud jsou vkládány NULL hodnoty gólù, tak se automaticky nastaví z výsledku
    IF :NEW.pocetgolydomacitym IS NULL OR :NEW.pocetgolyhostetym IS NULL THEN
        :NEW.pocetgolydomacitym := v_goly_domaci;
        :NEW.pocetgolyhostetym := v_goly_hoste;
    END IF;
    
    -- Kontrola shody poètu gólù se zadaným výsledkem
    IF v_goly_domaci != :NEW.POCETGOLYDOMACITYM OR v_goly_hoste != :NEW.POCETGOLYHOSTETYM THEN
        RAISE_APPLICATION_ERROR(-20003, 'Výsledek zápasu "' || :NEW.vysledek || '" nesouhlasí s poètem gólù "' 
            || :NEW.pocetgolydomacitym || ':' || :NEW.pocetgolyhostetym || '" !');
    END IF;
 
END;
/
ALTER TRIGGER "ST72870"."TRIG_KONTROLA_VYSLEDKU_VYSLEDKY_ZAPASU" ENABLE;
--------------------------------------------------------
--  DDL for Trigger TRIG_KONTROLA_ZAPASU_ODEHRANO
--------------------------------------------------------

  CREATE OR REPLACE EDITIONABLE TRIGGER "ST72870"."TRIG_KONTROLA_ZAPASU_ODEHRANO" 
BEFORE INSERT OR UPDATE ON vysledky_zapasu
FOR EACH ROW
DECLARE

    v_stav stav_zapasu.stavzapasu%TYPE;

BEGIN
    SELECT sz.stavZapasu
    INTO v_stav
    FROM ZAPASY z
    JOIN STAV_ZAPASU sz ON z.stav_zapasu_idstav = sz.idstav
    WHERE z.idzapas = :NEW.idzapas;

    IF v_stav <> 'Odehráno' THEN
        RAISE_APPLICATION_ERROR(-20001, 'Nelze zadat výsledek – zápas ještì nebyl odehrán.');
    END IF;
END;
/
ALTER TRIGGER "ST72870"."TRIG_KONTROLA_ZAPASU_ODEHRANO" ENABLE;
--------------------------------------------------------
--  DDL for Trigger TRIG_LOG_BINARNI_OBSAH
--------------------------------------------------------

  CREATE OR REPLACE EDITIONABLE TRIGGER "ST72870"."TRIG_LOG_BINARNI_OBSAH" 
AFTER INSERT OR UPDATE OR DELETE ON binarni_obsah

DECLARE 

    v_operace log_table.operace%TYPE;
    v_uzivatel log_table.uzivatel%TYPE;

BEGIN

    CASE
        WHEN INSERTING THEN
            v_operace := 'INSERT';

        WHEN UPDATING THEN
            v_operace := 'UPDATE';

        WHEN DELETING THEN
            v_operace := 'DELETE';

        ELSE
            v_operace := 'NEZNÁMÁ';

    END CASE;

    -- Nastavení uživatele, který provedl zmìnu v datech v dané tabulce
    v_uzivatel := pkg_user_context.get_user;
    
    IF v_uzivatel IS NULL THEN
        v_uzivatel := USER;
    END IF;

    pkg_log_table.sp_add_record_log_table(v_operace, SYSTIMESTAMP, v_uzivatel, 'BINARNI_OBSAH');
END;
/
ALTER TRIGGER "ST72870"."TRIG_LOG_BINARNI_OBSAH" ENABLE;
--------------------------------------------------------
--  DDL for Trigger TRIG_LOG_CLENOVE_KLUBU
--------------------------------------------------------

  CREATE OR REPLACE EDITIONABLE TRIGGER "ST72870"."TRIG_LOG_CLENOVE_KLUBU" 
AFTER INSERT OR UPDATE OR DELETE ON clenove_klubu

DECLARE 

    v_operace log_table.operace%TYPE;
    v_uzivatel log_table.uzivatel%TYPE;

BEGIN

    CASE
        WHEN INSERTING THEN
            v_operace := 'INSERT';

        WHEN UPDATING THEN
            v_operace := 'UPDATE';

        WHEN DELETING THEN
            v_operace := 'DELETE';

        ELSE
            v_operace := 'NEZNÁMÁ';

    END CASE;

    -- Nastavení uživatele, který provedl zmìnu v datech v dané tabulce
    v_uzivatel := pkg_user_context.get_user;
    
    IF v_uzivatel IS NULL THEN
        v_uzivatel := USER;
    END IF;

    pkg_log_table.sp_add_record_log_table(v_operace, SYSTIMESTAMP, v_uzivatel, 'CLENOVE_KLUBU');
END;
/
ALTER TRIGGER "ST72870"."TRIG_LOG_CLENOVE_KLUBU" ENABLE;
--------------------------------------------------------
--  DDL for Trigger TRIG_LOG_DISCLIPINARNI_OPATRENI
--------------------------------------------------------

  CREATE OR REPLACE EDITIONABLE TRIGGER "ST72870"."TRIG_LOG_DISCLIPINARNI_OPATRENI" 
AFTER INSERT OR UPDATE OR DELETE ON disciplinarni_opatreni

DECLARE 

    v_operace log_table.operace%TYPE;
    v_uzivatel log_table.uzivatel%TYPE;

BEGIN

    CASE
        WHEN INSERTING THEN
            v_operace := 'INSERT';

        WHEN UPDATING THEN
            v_operace := 'UPDATE';

        WHEN DELETING THEN
            v_operace := 'DELETE';

        ELSE
            v_operace := 'NEZNÁMÁ';

    END CASE;

    -- Nastavení uživatele, který provedl zmìnu v datech v dané tabulce
    v_uzivatel := pkg_user_context.get_user;
    
    IF v_uzivatel IS NULL THEN
        v_uzivatel := USER;
    END IF;

    pkg_log_table.sp_add_record_log_table(v_operace, SYSTIMESTAMP, v_uzivatel, 'DISCIPLINARNI_OPATRENI');
END;
/
ALTER TRIGGER "ST72870"."TRIG_LOG_DISCLIPINARNI_OPATRENI" ENABLE;
--------------------------------------------------------
--  DDL for Trigger TRIG_LOG_HRACI
--------------------------------------------------------

  CREATE OR REPLACE EDITIONABLE TRIGGER "ST72870"."TRIG_LOG_HRACI" 
AFTER INSERT OR UPDATE OR DELETE ON hraci

DECLARE 

    v_operace log_table.operace%TYPE;
    v_uzivatel log_table.uzivatel%TYPE;

BEGIN

    CASE
        WHEN INSERTING THEN
            v_operace := 'INSERT';

        WHEN UPDATING THEN
            v_operace := 'UPDATE';

        WHEN DELETING THEN
            v_operace := 'DELETE';

        ELSE
            v_operace := 'NEZNÁMÁ';

    END CASE;

    -- Nastavení uživatele, který provedl zmìnu v datech v dané tabulce
    v_uzivatel := pkg_user_context.get_user;
    
    IF v_uzivatel IS NULL THEN
        v_uzivatel := USER;
    END IF;

    pkg_log_table.sp_add_record_log_table(v_operace, SYSTIMESTAMP, v_uzivatel, 'HRACI');
END;
/
ALTER TRIGGER "ST72870"."TRIG_LOG_HRACI" ENABLE;
--------------------------------------------------------
--  DDL for Trigger TRIG_LOG_KONTRAKTY
--------------------------------------------------------

  CREATE OR REPLACE EDITIONABLE TRIGGER "ST72870"."TRIG_LOG_KONTRAKTY" 
AFTER INSERT OR UPDATE OR DELETE ON kontrakty

DECLARE 

    v_operace log_table.operace%TYPE;
    v_uzivatel log_table.uzivatel%TYPE;

BEGIN

    CASE
        WHEN INSERTING THEN
            v_operace := 'INSERT';

        WHEN UPDATING THEN
            v_operace := 'UPDATE';

        WHEN DELETING THEN
            v_operace := 'DELETE';

        ELSE
            v_operace := 'NEZNÁMÁ';

    END CASE;

    -- Nastavení uživatele, který provedl zmìnu v datech v dané tabulce
    v_uzivatel := pkg_user_context.get_user;
    
    IF v_uzivatel IS NULL THEN
        v_uzivatel := USER;
    END IF;

    pkg_log_table.sp_add_record_log_table(v_operace, SYSTIMESTAMP, v_uzivatel, 'KONTRAKTY');
END;
/
ALTER TRIGGER "ST72870"."TRIG_LOG_KONTRAKTY" ENABLE;
--------------------------------------------------------
--  DDL for Trigger TRIG_LOG_POZICE_HRAC
--------------------------------------------------------

  CREATE OR REPLACE EDITIONABLE TRIGGER "ST72870"."TRIG_LOG_POZICE_HRAC" 
AFTER INSERT OR UPDATE OR DELETE ON pozice_hrac

DECLARE 

    v_operace log_table.operace%TYPE;
    v_uzivatel log_table.uzivatel%TYPE;

BEGIN

    CASE
        WHEN INSERTING THEN
            v_operace := 'INSERT';

        WHEN UPDATING THEN
            v_operace := 'UPDATE';

        WHEN DELETING THEN
            v_operace := 'DELETE';

        ELSE
            v_operace := 'NEZNÁMÁ';

    END CASE;

    -- Nastavení uživatele, který provedl zmìnu v datech v dané tabulce
    v_uzivatel := pkg_user_context.get_user;
    
    IF v_uzivatel IS NULL THEN
        v_uzivatel := USER;
    END IF;

    pkg_log_table.sp_add_record_log_table(v_operace, SYSTIMESTAMP, v_uzivatel, 'POZICE_HRAC');
END;
/
ALTER TRIGGER "ST72870"."TRIG_LOG_POZICE_HRAC" ENABLE;
--------------------------------------------------------
--  DDL for Trigger TRIG_LOG_ROLE
--------------------------------------------------------

  CREATE OR REPLACE EDITIONABLE TRIGGER "ST72870"."TRIG_LOG_ROLE" 
AFTER INSERT OR UPDATE OR DELETE ON role

DECLARE 

    v_operace log_table.operace%TYPE;
    v_uzivatel log_table.uzivatel%TYPE;

BEGIN

    CASE
        WHEN INSERTING THEN
            v_operace := 'INSERT';

        WHEN UPDATING THEN
            v_operace := 'UPDATE';

        WHEN DELETING THEN
            v_operace := 'DELETE';

        ELSE
            v_operace := 'NEZNÁMÁ';

    END CASE;

    -- Nastavení uživatele, který provedl zmìnu v datech v dané tabulce
    v_uzivatel := pkg_user_context.get_user;
    
    IF v_uzivatel IS NULL THEN
        v_uzivatel := USER;
    END IF;

    pkg_log_table.sp_add_record_log_table(v_operace, SYSTIMESTAMP, v_uzivatel, 'ROLE');
END;
/
ALTER TRIGGER "ST72870"."TRIG_LOG_ROLE" ENABLE;
--------------------------------------------------------
--  DDL for Trigger TRIG_LOG_SOUTEZE
--------------------------------------------------------

  CREATE OR REPLACE EDITIONABLE TRIGGER "ST72870"."TRIG_LOG_SOUTEZE" 
AFTER INSERT OR UPDATE OR DELETE ON souteze

DECLARE 

    v_operace log_table.operace%TYPE;
    v_uzivatel log_table.uzivatel%TYPE;

BEGIN

    CASE
        WHEN INSERTING THEN
            v_operace := 'INSERT';

        WHEN UPDATING THEN
            v_operace := 'UPDATE';

        WHEN DELETING THEN
            v_operace := 'DELETE';

        ELSE
            v_operace := 'NEZNÁMÁ';

    END CASE;

    -- Nastavení uživatele, který provedl zmìnu v datech v dané tabulce
    v_uzivatel := pkg_user_context.get_user;
    
    IF v_uzivatel IS NULL THEN
        v_uzivatel := USER;
    END IF;

    pkg_log_table.sp_add_record_log_table(v_operace, SYSTIMESTAMP, v_uzivatel, 'SOUTEZE');
END;
/
ALTER TRIGGER "ST72870"."TRIG_LOG_SOUTEZE" ENABLE;
--------------------------------------------------------
--  DDL for Trigger TRIG_LOG_SPONZORI
--------------------------------------------------------

  CREATE OR REPLACE EDITIONABLE TRIGGER "ST72870"."TRIG_LOG_SPONZORI" 
AFTER INSERT OR UPDATE OR DELETE ON sponzori

DECLARE 

    v_operace log_table.operace%TYPE;
    v_uzivatel log_table.uzivatel%TYPE;

BEGIN

    CASE
        WHEN INSERTING THEN
            v_operace := 'INSERT';

        WHEN UPDATING THEN
            v_operace := 'UPDATE';

        WHEN DELETING THEN
            v_operace := 'DELETE';

        ELSE
            v_operace := 'NEZNÁMÁ';

    END CASE;

    -- Nastavení uživatele, který provedl zmìnu v datech v dané tabulce
    v_uzivatel := pkg_user_context.get_user;
    
    IF v_uzivatel IS NULL THEN
        v_uzivatel := USER;
    END IF;

    pkg_log_table.sp_add_record_log_table(v_operace, SYSTIMESTAMP, v_uzivatel, 'SPONZORI');
END;
/
ALTER TRIGGER "ST72870"."TRIG_LOG_SPONZORI" ENABLE;
--------------------------------------------------------
--  DDL for Trigger TRIG_LOG_SPONZORI_CLENOVE
--------------------------------------------------------

  CREATE OR REPLACE EDITIONABLE TRIGGER "ST72870"."TRIG_LOG_SPONZORI_CLENOVE" 
AFTER INSERT OR UPDATE OR DELETE ON sponzori_clenove

DECLARE 

    v_operace log_table.operace%TYPE;
    v_uzivatel log_table.uzivatel%TYPE;

BEGIN

    CASE
        WHEN INSERTING THEN
            v_operace := 'INSERT';

        WHEN UPDATING THEN
            v_operace := 'UPDATE';

        WHEN DELETING THEN
            v_operace := 'DELETE';

        ELSE
            v_operace := 'NEZNÁMÁ';

    END CASE;

    -- Nastavení uživatele, který provedl zmìnu v datech v dané tabulce
    v_uzivatel := pkg_user_context.get_user;
    
    IF v_uzivatel IS NULL THEN
        v_uzivatel := USER;
    END IF;

    pkg_log_table.sp_add_record_log_table(v_operace, SYSTIMESTAMP, v_uzivatel, 'SPONZORI_CLENOVE');
END;
/
ALTER TRIGGER "ST72870"."TRIG_LOG_SPONZORI_CLENOVE" ENABLE;
--------------------------------------------------------
--  DDL for Trigger TRIG_LOG_SPONZORI_SOUTEZE
--------------------------------------------------------

  CREATE OR REPLACE EDITIONABLE TRIGGER "ST72870"."TRIG_LOG_SPONZORI_SOUTEZE" 
AFTER INSERT OR UPDATE OR DELETE ON sponzori_souteze

DECLARE 

    v_operace log_table.operace%TYPE;
    v_uzivatel log_table.uzivatel%TYPE;

BEGIN

    CASE
        WHEN INSERTING THEN
            v_operace := 'INSERT';

        WHEN UPDATING THEN
            v_operace := 'UPDATE';

        WHEN DELETING THEN
            v_operace := 'DELETE';

        ELSE
            v_operace := 'NEZNÁMÁ';

    END CASE;

    -- Nastavení uživatele, který provedl zmìnu v datech v dané tabulce
    v_uzivatel := pkg_user_context.get_user;
    
    IF v_uzivatel IS NULL THEN
        v_uzivatel := USER;
    END IF;

    pkg_log_table.sp_add_record_log_table(v_operace, SYSTIMESTAMP, v_uzivatel, 'SPONZORI_SOUTEZE');
END;
/
ALTER TRIGGER "ST72870"."TRIG_LOG_SPONZORI_SOUTEZE" ENABLE;
--------------------------------------------------------
--  DDL for Trigger TRIG_LOG_STAV_ZAPASU
--------------------------------------------------------

  CREATE OR REPLACE EDITIONABLE TRIGGER "ST72870"."TRIG_LOG_STAV_ZAPASU" 
AFTER INSERT OR UPDATE OR DELETE ON stav_zapasu

DECLARE 

    v_operace log_table.operace%TYPE;
    v_uzivatel log_table.uzivatel%TYPE;

BEGIN

    CASE
        WHEN INSERTING THEN
            v_operace := 'INSERT';

        WHEN UPDATING THEN
            v_operace := 'UPDATE';

        WHEN DELETING THEN
            v_operace := 'DELETE';

        ELSE
            v_operace := 'NEZNÁMÁ';

    END CASE;

    -- Nastavení uživatele, který provedl zmìnu v datech v dané tabulce
    v_uzivatel := pkg_user_context.get_user;
    
    IF v_uzivatel IS NULL THEN
        v_uzivatel := USER;
    END IF;

    pkg_log_table.sp_add_record_log_table(v_operace, SYSTIMESTAMP, v_uzivatel, 'STAV_ZAPASU');
END;
/
ALTER TRIGGER "ST72870"."TRIG_LOG_STAV_ZAPASU" ENABLE;
--------------------------------------------------------
--  DDL for Trigger TRIG_LOG_TRENERI
--------------------------------------------------------

  CREATE OR REPLACE EDITIONABLE TRIGGER "ST72870"."TRIG_LOG_TRENERI" 
AFTER INSERT OR UPDATE OR DELETE ON treneri

DECLARE 

    v_operace log_table.operace%TYPE;
    v_uzivatel log_table.uzivatel%TYPE;

BEGIN

    CASE
        WHEN INSERTING THEN
            v_operace := 'INSERT';

        WHEN UPDATING THEN
            v_operace := 'UPDATE';

        WHEN DELETING THEN
            v_operace := 'DELETE';

        ELSE
            v_operace := 'NEZNÁMÁ';

    END CASE;

    -- Nastavení uživatele, který provedl zmìnu v datech v dané tabulce
    v_uzivatel := pkg_user_context.get_user;
    
    IF v_uzivatel IS NULL THEN
        v_uzivatel := USER;
    END IF;

    pkg_log_table.sp_add_record_log_table(v_operace, SYSTIMESTAMP, v_uzivatel, 'TRENERI');
END;
/
ALTER TRIGGER "ST72870"."TRIG_LOG_TRENERI" ENABLE;
--------------------------------------------------------
--  DDL for Trigger TRIG_LOG_TRENINKY
--------------------------------------------------------

  CREATE OR REPLACE EDITIONABLE TRIGGER "ST72870"."TRIG_LOG_TRENINKY" 
AFTER INSERT OR UPDATE OR DELETE ON treninky

DECLARE 

    v_operace log_table.operace%TYPE;
    v_uzivatel log_table.uzivatel%TYPE;

BEGIN

    CASE
        WHEN INSERTING THEN
            v_operace := 'INSERT';

        WHEN UPDATING THEN
            v_operace := 'UPDATE';

        WHEN DELETING THEN
            v_operace := 'DELETE';

        ELSE
            v_operace := 'NEZNÁMÁ';

    END CASE;

    -- Nastavení uživatele, který provedl zmìnu v datech v dané tabulce
    v_uzivatel := pkg_user_context.get_user;
    
    IF v_uzivatel IS NULL THEN
        v_uzivatel := USER;
    END IF;

    pkg_log_table.sp_add_record_log_table(v_operace, SYSTIMESTAMP, v_uzivatel, 'TRENINKY');
END;
/
ALTER TRIGGER "ST72870"."TRIG_LOG_TRENINKY" ENABLE;
--------------------------------------------------------
--  DDL for Trigger TRIG_LOG_TYP_SOUTEZ
--------------------------------------------------------

  CREATE OR REPLACE EDITIONABLE TRIGGER "ST72870"."TRIG_LOG_TYP_SOUTEZ" 
AFTER INSERT OR UPDATE OR DELETE ON typ_soutez

DECLARE 

    v_operace log_table.operace%TYPE;
    v_uzivatel log_table.uzivatel%TYPE;

BEGIN

    CASE
        WHEN INSERTING THEN
            v_operace := 'INSERT';

        WHEN UPDATING THEN
            v_operace := 'UPDATE';

        WHEN DELETING THEN
            v_operace := 'DELETE';

        ELSE
            v_operace := 'NEZNÁMÁ';

    END CASE;

    -- Nastavení uživatele, který provedl zmìnu v datech v dané tabulce
    v_uzivatel := pkg_user_context.get_user;
    
    IF v_uzivatel IS NULL THEN
        v_uzivatel := USER;
    END IF;

    pkg_log_table.sp_add_record_log_table(v_operace, SYSTIMESTAMP, v_uzivatel, 'TYP_SOUTEZ');
END;
/
ALTER TRIGGER "ST72870"."TRIG_LOG_TYP_SOUTEZ" ENABLE;
--------------------------------------------------------
--  DDL for Trigger TRIG_LOG_TYP_UZIVATELSKE_UCTY
--------------------------------------------------------

  CREATE OR REPLACE EDITIONABLE TRIGGER "ST72870"."TRIG_LOG_TYP_UZIVATELSKE_UCTY" 
AFTER INSERT OR UPDATE OR DELETE ON uzivatelske_ucty

DECLARE 

    v_operace log_table.operace%TYPE;
    v_uzivatel log_table.uzivatel%TYPE;

BEGIN

    CASE
        WHEN INSERTING THEN
            v_operace := 'INSERT';

        WHEN UPDATING THEN
            v_operace := 'UPDATE';

        WHEN DELETING THEN
            v_operace := 'DELETE';

        ELSE
            v_operace := 'NEZNÁMÁ';

    END CASE;

    -- Nastavení uživatele, který provedl zmìnu v datech v dané tabulce
    v_uzivatel := pkg_user_context.get_user;
    
    IF v_uzivatel IS NULL THEN
        v_uzivatel := USER;
    END IF;

    pkg_log_table.sp_add_record_log_table(v_operace, SYSTIMESTAMP, v_uzivatel, 'UZIVATELSKE_UCTY');
END;
/
ALTER TRIGGER "ST72870"."TRIG_LOG_TYP_UZIVATELSKE_UCTY" ENABLE;
--------------------------------------------------------
--  DDL for Trigger TRIG_LOG_TYP_VYSLEDKY_ZAPASU
--------------------------------------------------------

  CREATE OR REPLACE EDITIONABLE TRIGGER "ST72870"."TRIG_LOG_TYP_VYSLEDKY_ZAPASU" 
AFTER INSERT OR UPDATE OR DELETE ON vysledky_zapasu

DECLARE 

    v_operace log_table.operace%TYPE;
    v_uzivatel log_table.uzivatel%TYPE;

BEGIN

    CASE
        WHEN INSERTING THEN
            v_operace := 'INSERT';

        WHEN UPDATING THEN
            v_operace := 'UPDATE';

        WHEN DELETING THEN
            v_operace := 'DELETE';

        ELSE
            v_operace := 'NEZNÁMÁ';

    END CASE;

    -- Nastavení uživatele, který provedl zmìnu v datech v dané tabulce
    v_uzivatel := pkg_user_context.get_user;
    
    IF v_uzivatel IS NULL THEN
        v_uzivatel := USER;
    END IF;

    pkg_log_table.sp_add_record_log_table(v_operace, SYSTIMESTAMP, v_uzivatel, 'VYSLEDKY_ZAPASU');
END;
/
ALTER TRIGGER "ST72870"."TRIG_LOG_TYP_VYSLEDKY_ZAPASU" ENABLE;
--------------------------------------------------------
--  DDL for Trigger TRIG_LOG_TYP_ZAPASY
--------------------------------------------------------

  CREATE OR REPLACE EDITIONABLE TRIGGER "ST72870"."TRIG_LOG_TYP_ZAPASY" 
AFTER INSERT OR UPDATE OR DELETE ON zapasy

DECLARE 

    v_operace log_table.operace%TYPE;
    v_uzivatel log_table.uzivatel%TYPE;

BEGIN

    CASE
        WHEN INSERTING THEN
            v_operace := 'INSERT';

        WHEN UPDATING THEN
            v_operace := 'UPDATE';

        WHEN DELETING THEN
            v_operace := 'DELETE';

        ELSE
            v_operace := 'NEZNÁMÁ';

    END CASE;

    -- Nastavení uživatele, který provedl zmìnu v datech v dané tabulce
    v_uzivatel := pkg_user_context.get_user;
    
    IF v_uzivatel IS NULL THEN
        v_uzivatel := USER;
    END IF;

    pkg_log_table.sp_add_record_log_table(v_operace, SYSTIMESTAMP, v_uzivatel, 'ZAPASY');
END;
/
ALTER TRIGGER "ST72870"."TRIG_LOG_TYP_ZAPASY" ENABLE;
--------------------------------------------------------
--  DDL for Trigger TRIG_SEKV_CLENKLUBU
--------------------------------------------------------

  CREATE OR REPLACE EDITIONABLE TRIGGER "ST72870"."TRIG_SEKV_CLENKLUBU" BEFORE
    INSERT ON clenove_klubu
    FOR EACH ROW
     WHEN ( new.idclenklubu IS NULL ) BEGIN
    :new.idclenklubu := sekv_clenklubu.nextval;
END;

/
ALTER TRIGGER "ST72870"."TRIG_SEKV_CLENKLUBU" ENABLE;
--------------------------------------------------------
--  DDL for Trigger TRIG_SEKV_HRAC_CLENKLUBU
--------------------------------------------------------

  CREATE OR REPLACE EDITIONABLE TRIGGER "ST72870"."TRIG_SEKV_HRAC_CLENKLUBU" BEFORE
    INSERT ON hraci
    FOR EACH ROW
     WHEN ( new.idclenklubu IS NULL ) BEGIN
    :new.idclenklubu := sekv_hrac_clenklubu.nextval;
END;

/
ALTER TRIGGER "ST72870"."TRIG_SEKV_HRAC_CLENKLUBU" ENABLE;
--------------------------------------------------------
--  DDL for Trigger TRIG_SEKV_LOG_TABLE
--------------------------------------------------------

  CREATE OR REPLACE EDITIONABLE TRIGGER "ST72870"."TRIG_SEKV_LOG_TABLE" BEFORE
    INSERT ON log_table
    FOR EACH ROW
     WHEN ( new.idlog IS NULL ) BEGIN
    :new.idlog := sekv_log_table.nextval;
END;
/
ALTER TRIGGER "ST72870"."TRIG_SEKV_LOG_TABLE" ENABLE;
--------------------------------------------------------
--  DDL for Trigger TRIG_SEKV_OBSAH
--------------------------------------------------------

  CREATE OR REPLACE EDITIONABLE TRIGGER "ST72870"."TRIG_SEKV_OBSAH" 
BEFORE INSERT ON Binarni_Obsah 
FOR EACH ROW 
 WHEN (NEW.idBinarniObsah IS NULL) BEGIN 
    :NEW.idBinarniObsah := sekv_obsah.NEXTVAL; 
END;

/
ALTER TRIGGER "ST72870"."TRIG_SEKV_OBSAH" ENABLE;
--------------------------------------------------------
--  DDL for Trigger TRIG_SEKV_OPATRENI
--------------------------------------------------------

  CREATE OR REPLACE EDITIONABLE TRIGGER "ST72870"."TRIG_SEKV_OPATRENI" BEFORE
    INSERT ON disciplinarni_opatreni
    FOR EACH ROW
     WHEN ( new.idopatreni IS NULL ) BEGIN
    :new.idopatreni := sekv_opatreni.nextval;
END;

/
ALTER TRIGGER "ST72870"."TRIG_SEKV_OPATRENI" ENABLE;
--------------------------------------------------------
--  DDL for Trigger TRIG_SEKV_POZICE
--------------------------------------------------------

  CREATE OR REPLACE EDITIONABLE TRIGGER "ST72870"."TRIG_SEKV_POZICE" BEFORE
    INSERT ON pozice_hrac
    FOR EACH ROW
     WHEN ( new.id_pozice IS NULL ) BEGIN
    :new.id_pozice := sekv_pozice.nextval;
END;
/
ALTER TRIGGER "ST72870"."TRIG_SEKV_POZICE" ENABLE;
--------------------------------------------------------
--  DDL for Trigger TRIG_SEKV_ROLE
--------------------------------------------------------

  CREATE OR REPLACE EDITIONABLE TRIGGER "ST72870"."TRIG_SEKV_ROLE" BEFORE
    INSERT ON role
    FOR EACH ROW
     WHEN ( new.idrole IS NULL ) BEGIN
    :new.idrole := sekv_role.nextval;
END;
/
ALTER TRIGGER "ST72870"."TRIG_SEKV_ROLE" ENABLE;
--------------------------------------------------------
--  DDL for Trigger TRIG_SEKV_SOUTEZ
--------------------------------------------------------

  CREATE OR REPLACE EDITIONABLE TRIGGER "ST72870"."TRIG_SEKV_SOUTEZ" BEFORE
    INSERT ON souteze
    FOR EACH ROW
     WHEN ( new.idsoutez IS NULL ) BEGIN
    :new.idsoutez := sekv_soutez.nextval;
END;

/
ALTER TRIGGER "ST72870"."TRIG_SEKV_SOUTEZ" ENABLE;
--------------------------------------------------------
--  DDL for Trigger TRIG_SEKV_SPONZOR
--------------------------------------------------------

  CREATE OR REPLACE EDITIONABLE TRIGGER "ST72870"."TRIG_SEKV_SPONZOR" BEFORE
    INSERT ON sponzori
    FOR EACH ROW
     WHEN ( new.idsponzor IS NULL ) BEGIN
    :new.idsponzor := sekv_sponzor.nextval;
END;

/
ALTER TRIGGER "ST72870"."TRIG_SEKV_SPONZOR" ENABLE;
--------------------------------------------------------
--  DDL for Trigger TRIG_SEKV_STAVZAPASU
--------------------------------------------------------

  CREATE OR REPLACE EDITIONABLE TRIGGER "ST72870"."TRIG_SEKV_STAVZAPASU" BEFORE
    INSERT ON stav_zapasu
    FOR EACH ROW
     WHEN ( new.idstav IS NULL ) BEGIN
    :new.idstav := sekv_stavzapasu.nextval;
END;
/
ALTER TRIGGER "ST72870"."TRIG_SEKV_STAVZAPASU" ENABLE;
--------------------------------------------------------
--  DDL for Trigger TRIG_SEKV_TRENER_CLENKLUBU
--------------------------------------------------------

  CREATE OR REPLACE EDITIONABLE TRIGGER "ST72870"."TRIG_SEKV_TRENER_CLENKLUBU" BEFORE
    INSERT ON treneri
    FOR EACH ROW
     WHEN ( new.idclenklubu IS NULL ) BEGIN
    :new.idclenklubu := sekv_trener_clenklubu.nextval;
END;

/
ALTER TRIGGER "ST72870"."TRIG_SEKV_TRENER_CLENKLUBU" ENABLE;
--------------------------------------------------------
--  DDL for Trigger TRIG_SEKV_TRENINK
--------------------------------------------------------

  CREATE OR REPLACE EDITIONABLE TRIGGER "ST72870"."TRIG_SEKV_TRENINK" BEFORE
    INSERT ON treninky
    FOR EACH ROW
     WHEN ( new.idtrenink IS NULL ) BEGIN
    :new.idtrenink := sekv_trenink.nextval;
END;

/
ALTER TRIGGER "ST72870"."TRIG_SEKV_TRENINK" ENABLE;
--------------------------------------------------------
--  DDL for Trigger TRIG_SEKV_TYPSOUTEZE
--------------------------------------------------------

  CREATE OR REPLACE EDITIONABLE TRIGGER "ST72870"."TRIG_SEKV_TYPSOUTEZE" BEFORE
    INSERT ON typ_soutez
    FOR EACH ROW
     WHEN ( new.idtypsouteze IS NULL ) BEGIN
    :new.idtypsouteze := sekv_typsouteze.nextval;
END;
/
ALTER TRIGGER "ST72870"."TRIG_SEKV_TYPSOUTEZE" ENABLE;
--------------------------------------------------------
--  DDL for Trigger TRIG_SEKV_UZIVATELSKE_UCTY
--------------------------------------------------------

  CREATE OR REPLACE EDITIONABLE TRIGGER "ST72870"."TRIG_SEKV_UZIVATELSKE_UCTY" BEFORE
    INSERT ON uzivatelske_ucty
    FOR EACH ROW
     WHEN ( new.iduzivatelskyucet IS NULL ) BEGIN
    :new.iduzivatelskyucet := sekv_uzivatelske_ucty.nextval;
END;
/
ALTER TRIGGER "ST72870"."TRIG_SEKV_UZIVATELSKE_UCTY" ENABLE;
--------------------------------------------------------
--  DDL for Trigger TRIG_SEKV_ZAPAS
--------------------------------------------------------

  CREATE OR REPLACE EDITIONABLE TRIGGER "ST72870"."TRIG_SEKV_ZAPAS" BEFORE
    INSERT ON zapasy
    FOR EACH ROW
     WHEN ( new.idzapas IS NULL ) BEGIN
    :new.idzapas := sekv_zapas.nextval;
END;

/
ALTER TRIGGER "ST72870"."TRIG_SEKV_ZAPAS" ENABLE;
--------------------------------------------------------
--  DDL for Trigger TRIG_SNIZ_PLAT_HRACE_KONTRAKTY
--------------------------------------------------------

  CREATE OR REPLACE EDITIONABLE TRIGGER "ST72870"."TRIG_SNIZ_PLAT_HRACE_KONTRAKTY" 
AFTER INSERT OR UPDATE OF idopatreni ON hraci
FOR EACH ROW
 WHEN (NEW.idopatreni IS NOT NULL AND OLD.idopatreni IS NULL) DECLARE

    v_novy_plat kontrakty.plat%TYPE;
    v_stary_plat kontrakty.plat%TYPE;
    v_pocet NUMBER := 0;

BEGIN

    -- Poèet disciplinárních opatøení hráèe
    SELECT COUNT(*) INTO v_pocet
    FROM disciplinarni_opatreni opat
    WHERE idopatreni = :NEW.idopatreni;

    IF v_pocet > 0 THEN

        SELECT plat INTO v_stary_plat
        FROM kontrakty
        WHERE idclenklubu = :NEW.idclenklubu
          AND (datumKonce IS NULL OR datumKonce > SYSDATE) AND ROWNUM = 1;

        -- Výpoèet nového platu
        v_novy_plat := v_stary_plat * 0.95;

        -- Aktualizace kontraktu
        UPDATE kontrakty
        SET plat = v_novy_plat
        WHERE idclenklubu = :NEW.idclenklubu AND (datumkonce IS NULL OR datumkonce > SYSDATE);
    END IF;

    EXCEPTION
            -- Pokud hráè nemá kontrakt, nic se nestane
            WHEN NO_DATA_FOUND THEN
                NULL;
END;
/
ALTER TRIGGER "ST72870"."TRIG_SNIZ_PLAT_HRACE_KONTRAKTY" ENABLE;
--------------------------------------------------------
--  DDL for Package PKG_BINARNI_OBSAH
--------------------------------------------------------

  CREATE OR REPLACE EDITIONABLE PACKAGE "ST72870"."PKG_BINARNI_OBSAH" AS

  ------------------------------------------------------------------
  -- Pøidání nového binárního souboru
  ------------------------------------------------------------------
  PROCEDURE SP_ADD_OBSAH(
    v_nazev_souboru     IN BINARNI_OBSAH.NAZEVSOUBORU%TYPE,
    v_typ_souboru       IN BINARNI_OBSAH.TYPSOUBORU%TYPE,
    v_pripona_souboru   IN BINARNI_OBSAH.PRIPONASOUBORU%TYPE,
    v_obsah             IN BINARNI_OBSAH.OBSAH%TYPE,
    v_operace           IN BINARNI_OBSAH.OPERACE%TYPE,
    v_id_uzivatel       IN BINARNI_OBSAH.IDUZIVATELSKYUCET%TYPE
  );

  ------------------------------------------------------------------
  -- Aktualizace existujícího souboru
  ------------------------------------------------------------------
  PROCEDURE SP_UPDATE_OBSAH(
    v_id_obsah          IN BINARNI_OBSAH.IDBINARNIOBSAH%TYPE,
    v_obsah             IN BINARNI_OBSAH.OBSAH%TYPE,
    v_operace           IN BINARNI_OBSAH.OPERACE%TYPE,
    v_id_uzivatel       IN BINARNI_OBSAH.IDUZIVATELSKYUCET%TYPE
  );
  
  ------------------------------------------------------------------
  -- Pøejmenování souboru
  ------------------------------------------------------------------
  PROCEDURE SP_RENAME_OBSAH(
    v_id_obsah    IN BINARNI_OBSAH.IDBINARNIOBSAH%TYPE,
    v_novy_nazev  IN BINARNI_OBSAH.NAZEVSOUBORU%TYPE,
    v_id_uzivatel IN BINARNI_OBSAH.IDUZIVATELSKYUCET%TYPE

);

  ------------------------------------------------------------------
  -- Smazání souboru podle ID
  ------------------------------------------------------------------
  PROCEDURE SP_DELETE_OBSAH(
    v_id_obsah          IN BINARNI_OBSAH.IDBINARNIOBSAH%TYPE
  );

END PKG_BINARNI_OBSAH;

/
--------------------------------------------------------
--  DDL for Package PKG_HRACI
--------------------------------------------------------

  CREATE OR REPLACE EDITIONABLE PACKAGE "ST72870"."PKG_HRACI" AS
  ------------------------------------------------------------------
  -- Pøidání hráèe
  ------------------------------------------------------------------
  PROCEDURE SP_ADD_HRAC(
      v_rodne_cislo      IN CLENOVE_KLUBU.RODNE_CISLO%TYPE,
      v_jmeno            IN CLENOVE_KLUBU.JMENO%TYPE,
      v_prijmeni         IN CLENOVE_KLUBU.PRIJMENI%TYPE,
      v_telefonni_cislo  IN CLENOVE_KLUBU.TELEFONNICISLO%TYPE,
      v_id_pozice        IN HRACI.ID_POZICE%TYPE,
      v_pocet_golu       IN HRACI.POCETVSTRELENYCHGOLU%TYPE DEFAULT 0,
      v_pocet_zlute      IN HRACI.POCET_ZLUTYCH_KARET%TYPE DEFAULT 0,
      v_pocet_cervene    IN HRACI.POCET_CERVENYCH_KARET%TYPE DEFAULT 0,
      v_datum_opatreni   IN DISCIPLINARNI_OPATRENI.DATUMOPATRENI%TYPE DEFAULT NULL,
      v_delka_trestu     IN DISCIPLINARNI_OPATRENI.DELKATRESTU%TYPE DEFAULT NULL,
      v_duvod            IN DISCIPLINARNI_OPATRENI.DUVOD%TYPE DEFAULT NULL
  );

  ------------------------------------------------------------------
  -- Aktualizace hráèe
  ------------------------------------------------------------------
  PROCEDURE SP_UPDATE_HRAC(
      v_rodne_cislo_puvodni IN CLENOVE_KLUBU.RODNE_CISLO%TYPE,
      v_rodne_cislo      IN CLENOVE_KLUBU.RODNE_CISLO%TYPE,
      v_jmeno            IN CLENOVE_KLUBU.JMENO%TYPE,
      v_prijmeni         IN CLENOVE_KLUBU.PRIJMENI%TYPE,
      v_telefonni_cislo  IN CLENOVE_KLUBU.TELEFONNICISLO%TYPE,
      v_id_pozice        IN HRACI.ID_POZICE%TYPE,
      v_pocet_golu       IN HRACI.POCETVSTRELENYCHGOLU%TYPE,
      v_pocet_zlute      IN HRACI.POCET_ZLUTYCH_KARET%TYPE,
      v_pocet_cervene    IN HRACI.POCET_CERVENYCH_KARET%TYPE,
      v_datum_opatreni   IN DISCIPLINARNI_OPATRENI.DATUMOPATRENI%TYPE DEFAULT NULL,
      v_delka_trestu     IN DISCIPLINARNI_OPATRENI.DELKATRESTU%TYPE DEFAULT NULL,
      v_duvod            IN DISCIPLINARNI_OPATRENI.DUVOD%TYPE DEFAULT NULL
  );
  ------------------------------------------------------------------
  -- Odebrání hráèe
  ------------------------------------------------------------------
  PROCEDURE SP_ODEBER_HRACE(
      v_rodne_cislo IN CLENOVE_KLUBU.RODNE_CISLO%TYPE
  );

  ------------------------------------------------------------------
  -- Funkce
  ------------------------------------------------------------------
  FUNCTION F_GOLY_PROCENTA_POZICE(
      p_pozice IN HRACI.ID_POZICE%TYPE
  ) RETURN NUMBER;

END PKG_HRACI;

/
--------------------------------------------------------
--  DDL for Package PKG_KONTRAKTY
--------------------------------------------------------

  CREATE OR REPLACE EDITIONABLE PACKAGE "ST72870"."PKG_KONTRAKTY" AS 

-- Procedura pro vytvoøení nového kontraktu
PROCEDURE SP_ADD_KONTRAKT
(
    v_id_clen IN kontrakty.idclenklubu%TYPE
,   v_datum_zacatku IN kontrakty.datumzacatku%TYPE
,   v_datum_konce IN kontrakty.datumkonce%TYPE
,   v_plat IN kontrakty.plat%TYPE
,   v_cislo_na_agenta IN kontrakty.cislonaagenta%TYPE
,   v_vystupni_klauzule IN kontrakty.vystupniklauzule%TYPE
);


-- Procedura pro odebrání urèitého kontraktu
PROCEDURE SP_ODEBER_KONTRAKT
(
    v_id_clen IN kontrakty.idclenklubu%TYPE
);


-- Procedura pro editaci vybraného kontraktu
PROCEDURE SP_UPDATE_KONTRAKT
(
    v_id_clen IN kontrakty.idclenklubu%TYPE
,   v_datum_zacatku IN kontrakty.datumzacatku%TYPE
,   v_datum_konce IN kontrakty.datumkonce%TYPE
,   v_plat IN kontrakty.plat%TYPE
,   v_cislo_na_agenta IN kontrakty.cislonaagenta%TYPE
,   v_vystupni_klauzule IN kontrakty.vystupniklauzule%TYPE
);


-- Procedura pro kontrolu, zda není datum konce døíve než zaèátek
PROCEDURE SP_KONTROLA_DATUMU
(
    v_zacatek kontrakty.datumzacatku%TYPE
,   v_konec kontrakty.datumkonce%TYPE
);


-- Procedura pro zvýšení platu hráèe o 5% pokud má minimálnì 3 sponzori
PROCEDURE SP_ZVYS_PLAT_HRACE;


-- Procedura pro nalezení kontraktù, kterým konèí platnost za urèité èasové období (DEFAULTNÌ 30 dní)
PROCEDURE SP_KONTROLA_KONCICICH_KONTRAKTU 
(
    v_pocet_dni_do_konce_platnosti IN NUMBER DEFAULT 30
,   v_json_vystup OUT CLOB 
);

END PKG_KONTRAKTY;

/
--------------------------------------------------------
--  DDL for Package PKG_LOG_TABLE
--------------------------------------------------------

  CREATE OR REPLACE EDITIONABLE PACKAGE "ST72870"."PKG_LOG_TABLE" AS 

-- Procedura pro zapisování zmìn v datech v jednotlivých tabulkách
PROCEDURE SP_ADD_RECORD_LOG_TABLE
(
    v_operace IN log_table.operace%TYPE
,   v_cas IN log_table.cas%TYPE
,   v_uzivatel IN log_table.uzivatel%TYPE
,   v_tabulka IN log_table.tabulka%TYPE
);

END PKG_LOG_TABLE;

/
--------------------------------------------------------
--  DDL for Package PKG_REGISTRACE
--------------------------------------------------------

  CREATE OR REPLACE EDITIONABLE PACKAGE "ST72870"."PKG_REGISTRACE" AS

    PROCEDURE SP_ADD_UZIVATEL (
        v_uzivatelske_jmeno   IN UZIVATELSKE_UCTY.UZIVATELSKEJMENO%TYPE,
        v_heslo               IN UZIVATELSKE_UCTY.HESLO%TYPE,
        v_posledni_prihlaseni IN UZIVATELSKE_UCTY.POSLEDNIPRIHLASENI%TYPE,
        v_idrole              IN UZIVATELSKE_UCTY.IDROLE%TYPE,
        v_salt                IN UZIVATELSKE_UCTY.SALT%TYPE,
        v_email               IN UZIVATELSKE_UCTY.EMAIL%TYPE
    );

    PROCEDURE SP_DELETE_UZIVATEL (
        v_uzivatelske_jmeno IN UZIVATELSKE_UCTY.UZIVATELSKEJMENO%TYPE
    );

    PROCEDURE SP_UPDATE_UZIVATEL (
        v_stare_jmeno        IN UZIVATELSKE_UCTY.UZIVATELSKEJMENO%TYPE,
        v_nove_jmeno         IN UZIVATELSKE_UCTY.UZIVATELSKEJMENO%TYPE,
        v_email             IN UZIVATELSKE_UCTY.EMAIL%TYPE,
        v_heslo             IN UZIVATELSKE_UCTY.HESLO%TYPE,
        v_salt              IN UZIVATELSKE_UCTY.SALT%TYPE,
        v_idrole            IN UZIVATELSKE_UCTY.IDROLE%TYPE
    );
    
    PROCEDURE SP_NASTAV_CLENA(
    v_uzivatelske_jmeno IN UZIVATELSKE_UCTY.UZIVATELSKEJMENO%TYPE,
    v_id_clen           IN UZIVATELSKE_UCTY.CLEN_KLUBU_IDCLENKLUBU%TYPE
);

    PROCEDURE SP_UPDATE_POSLEDNI_PRIHLASENI (
    v_uzivatelske_jmeno IN UZIVATELSKE_UCTY.UZIVATELSKEJMENO%TYPE
);


END PKG_REGISTRACE;

/
--------------------------------------------------------
--  DDL for Package PKG_SOUTEZE
--------------------------------------------------------

  CREATE OR REPLACE EDITIONABLE PACKAGE "ST72870"."PKG_SOUTEZE" AS 

-- Procedura pro vytvoøení nové soutìže
PROCEDURE SP_ADD_SOUTEZ
(
    v_id_typ_souteze IN souteze.idtypsouteze%TYPE
,   v_datum_zacatek IN souteze.startdatum%TYPE
,   v_datum_konec IN souteze.konecdatum%TYPE
);


-- Procedura pro odebrání urèité soutìže
PROCEDURE SP_ODEBER_SOUTEZ 
(
    v_id_soutez IN souteze.idsoutez%TYPE
);


-- Procedura pro editaci vybrané soutìže
PROCEDURE SP_UPDATE_SOUTEZ
(
    v_id_soutez IN souteze.idsoutez%TYPE
,   v_id_typ_souteze IN souteze.idtypsouteze%TYPE
,   v_datum_zacatek IN souteze.startdatum%TYPE
,   v_datum_konec IN souteze.konecdatum%TYPE
);


-- Procedura pro kontrolu, zda není datum konce døíve než zaèátek
PROCEDURE SP_KONTROLA_DATUMU
(
    v_zacatek souteze.startdatum%TYPE
,   v_konec souteze.konecdatum%TYPE
);


-- Procedura pro výpis hierarchie soutìží pomocí hierarchického dotazu
PROCEDURE SP_VYPIS_HIERARCHII_SOUTEZI
(
    v_cursor OUT SYS_REFCURSOR
);

END PKG_SOUTEZE;

/
--------------------------------------------------------
--  DDL for Package PKG_SPONZORI
--------------------------------------------------------

  CREATE OR REPLACE EDITIONABLE PACKAGE "ST72870"."PKG_SPONZORI" AS 

-- Procedura pro vytvoøení nového sponzora
PROCEDURE SP_ADD_SPONZOR 
(
    v_jmeno IN sponzori.jmeno%TYPE
,   v_sponzorovana_castka IN sponzori.sponzorovanacastka%TYPE
);


-- Procedura pro odebrání urèitého sponzora
PROCEDURE SP_ODEBER_SPONZOR 
(
    v_id_sponzor IN sponzori.idsponzor%TYPE
);


-- Procedura pro editaci vybraného sponzora
PROCEDURE SP_UPDATE_SPONZOR 
(
    v_id_sponzor IN sponzori.idsponzor%TYPE
,   v_jmeno IN sponzori.jmeno%TYPE
,   v_sponzorovana_castka IN sponzori.sponzorovanacastka%TYPE
);

END PKG_SPONZORI;

/
--------------------------------------------------------
--  DDL for Package PKG_SPONZORI_CLENOVE
--------------------------------------------------------

  CREATE OR REPLACE EDITIONABLE PACKAGE "ST72870"."PKG_SPONZORI_CLENOVE" AS 

-- Procedura pro vytvoøení nové vazby SPONZORI-CLENOVE
PROCEDURE SP_ADD_SPONZORI_CLENOVE 
(
  v_id_sponzor IN sponzori_clenove.idsponzor%TYPE
, v_id_clen IN sponzori_clenove.idclenklubu%TYPE
);


-- Procedura pro odebrání urèité vazby SPONZORI-CLENOVE
PROCEDURE SP_ODEBER_SPONZORI_CLENOVE 
(
  v_id_sponzor IN sponzori_clenove.idsponzor%TYPE
, v_id_clen IN sponzori_clenove.idclenklubu%TYPE
);


-- Procedura pro odebrání všech vazeb SPONZORI-CLENOVE urèitého sponzora
PROCEDURE SP_ODEBER_VSECHNY_SPONZORI_CLENOVE 
(
  v_id_sponzor IN sponzori_clenove.idsponzor%TYPE
);

END PKG_SPONZORI_CLENOVE;

/
--------------------------------------------------------
--  DDL for Package PKG_SPONZORI_SOUTEZE
--------------------------------------------------------

  CREATE OR REPLACE EDITIONABLE PACKAGE "ST72870"."PKG_SPONZORI_SOUTEZE" AS 

-- Procedura pro vytvoøení nové vazby SPONZORI-SOUTEZE
PROCEDURE SP_ADD_SPONZORI_SOUTEZE 
(
    v_id_sponzor IN sponzori_souteze.idsponzor%TYPE
,   v_id_soutez IN sponzori_souteze.idsoutez%TYPE
);

-- Procedura pro odebrání urèité vazby SPONZORI-SOUTEZE
PROCEDURE SP_ODEBER_SPONZORI_SOUTEZE 
(
    v_id_sponzor IN sponzori_souteze.idsponzor%TYPE
,   v_id_soutez IN sponzori_souteze.idsoutez%TYPE
);


-- Procedura pro odebrání všech vazeb SPONZORI-SOUTEZE urèitého sponzora
PROCEDURE SP_ODEBER_VSECHNY_SPONZORI_SOUTEZE
(
    v_id_sponzor IN sponzori_souteze.idsponzor%TYPE
);

END PKG_SPONZORI_SOUTEZE;

/
--------------------------------------------------------
--  DDL for Package PKG_TRENERI
--------------------------------------------------------

  CREATE OR REPLACE EDITIONABLE PACKAGE "ST72870"."PKG_TRENERI" AS
    ------------------------------------------------------------------
    -- Pøidá nového trenéra
    ------------------------------------------------------------------
    PROCEDURE SP_ADD_TRENERI(
        v_rodne_cislo       IN CLENOVE_KLUBU.RODNE_CISLO%TYPE,
        v_jmeno             IN CLENOVE_KLUBU.JMENO%TYPE,
        v_prijmeni          IN CLENOVE_KLUBU.PRIJMENI%TYPE,
        v_telefonni_cislo   IN CLENOVE_KLUBU.TELEFONNICISLO%TYPE,
        v_trenerska_licence IN TRENERI.TRENERSKALICENCE%TYPE,
        v_specializace      IN TRENERI.SPECIALIZACE%TYPE DEFAULT NULL,
        v_pocet_let_praxe   IN TRENERI.POCETLETPRAXE%TYPE
    );

    ------------------------------------------------------------------
    -- Edituje/Aktualizuje údaje o trenérovi
    ------------------------------------------------------------------
    PROCEDURE SP_UPDATE_TRENERI(
        v_rodne_cislo_puvodni IN CLENOVE_KLUBU.RODNE_CISLO%TYPE,
        v_rodne_cislo       IN CLENOVE_KLUBU.RODNE_CISLO%TYPE,
        v_jmeno             IN CLENOVE_KLUBU.JMENO%TYPE,
        v_prijmeni          IN CLENOVE_KLUBU.PRIJMENI%TYPE,
        v_telefonni_cislo   IN CLENOVE_KLUBU.TELEFONNICISLO%TYPE,
        v_trenerska_licence IN TRENERI.TRENERSKALICENCE%TYPE,
        v_specializace      IN TRENERI.SPECIALIZACE%TYPE DEFAULT NULL,
        v_pocet_let_praxe   IN TRENERI.POCETLETPRAXE%TYPE
    );

    ------------------------------------------------------------------
    -- Smaže trenéra z databáze
    ------------------------------------------------------------------
    PROCEDURE SP_ODEBER_TRENERI(
        v_rodne_cislo IN CLENOVE_KLUBU.RODNE_CISLO%TYPE
    );
    
    ------------------------------------------------------------------
    -- Funkce pro generování TOP 3 trenérù podle praxe (BLOB)
    ------------------------------------------------------------------
    FUNCTION F_TOP3_TRENERI_BLOB RETURN BLOB;
    
END PKG_TRENERI;

/
--------------------------------------------------------
--  DDL for Package PKG_TRENINKY
--------------------------------------------------------

  CREATE OR REPLACE EDITIONABLE PACKAGE "ST72870"."PKG_TRENINKY" AS

   ------------------------------------------------------------------
    -- Pøidá nového tréninku
    ------------------------------------------------------------------
    PROCEDURE SP_ADD_TRENINK (
        v_prijmeni     IN CLENOVE_KLUBU.PRIJMENI%TYPE,
        v_rodne_cislo  IN CLENOVE_KLUBU.RODNE_CISLO%TYPE,
        v_datum        IN TRENINKY.DATUM%TYPE,
        v_misto        IN TRENINKY.MISTO%TYPE,
        v_popis        IN TRENINKY.POPIS%TYPE DEFAULT NULL
    );

    ------------------------------------------------------------------
    -- Edituje/Aktualizuje údaje o tréninku
    ------------------------------------------------------------------
    PROCEDURE SP_UPDATE_TRENINK (
        v_prijmeni     IN CLENOVE_KLUBU.PRIJMENI%TYPE,
        v_rodne_cislo  IN CLENOVE_KLUBU.RODNE_CISLO%TYPE,
        v_datum        IN TRENINKY.DATUM%TYPE,
        v_misto        IN TRENINKY.MISTO%TYPE,
        v_popis        IN TRENINKY.POPIS%TYPE DEFAULT NULL
    );

    ------------------------------------------------------------------
    -- Smaže trénink z databáze
    ------------------------------------------------------------------
    PROCEDURE SP_DELETE_TRENINK (
        v_rodne_cislo  IN CLENOVE_KLUBU.RODNE_CISLO%TYPE
    );
    
    
    ------------------------------------------------------------------
    -- Vypoèítá zatížení tréninkových dnù v daném mìsíci
    ------------------------------------------------------------------
    PROCEDURE SP_VYPOCITEJ_ZATEZ_TRENINK_DNU (
    v_datum IN treninky.datum%type,
    v_info OUT VARCHAR2
    );

END PKG_TRENINKY;

/
--------------------------------------------------------
--  DDL for Package PKG_USER_CONTEXT
--------------------------------------------------------

  CREATE OR REPLACE EDITIONABLE PACKAGE "ST72870"."PKG_USER_CONTEXT" AS
    
    PROCEDURE set_user(v_user VARCHAR2);
    
    
    FUNCTION get_user RETURN VARCHAR2;
    
END pkg_user_context;

/
--------------------------------------------------------
--  DDL for Package PKG_VYSLEDKY_ZAPASU
--------------------------------------------------------

  CREATE OR REPLACE EDITIONABLE PACKAGE "ST72870"."PKG_VYSLEDKY_ZAPASU" AS 

-- Procedura pro vytvoøení nového výsledku
PROCEDURE SP_ADD_VYSLEDEK_ZAPASU
(
    v_id_zapas IN vysledky_zapasu.idzapas%TYPE
,   v_vysledek IN vysledky_zapasu.vysledek%TYPE
,   v_pocet_zlutych_karet IN vysledky_zapasu.pocetzlutychkaret%TYPE
,   v_pocet_cervenych_karet IN vysledky_zapasu.pocetcervenychkaret%TYPE
,   v_pocet_goly_domaci IN vysledky_zapasu.pocetgolydomacitym%TYPE
,   v_pocet_goly_hoste IN vysledky_zapasu.pocetgolyhostetym%TYPE
);


-- Procedura pro odebrání urèitého výsledku
PROCEDURE SP_ODEBER_VYSLEDEK_ZAPASU
(
    v_id_zapas IN vysledky_zapasu.idzapas%TYPE
);


-- Procedura pro editaci vybraného výsledku
PROCEDURE SP_UPDATE_VYSLEDEK_ZAPASU
(
    v_id_zapas IN vysledky_zapasu.idzapas%TYPE
,   v_vysledek IN vysledky_zapasu.vysledek%TYPE
,   v_pocet_zlutych_karet IN vysledky_zapasu.pocetzlutychkaret%TYPE
,   v_pocet_cervenych_karet IN vysledky_zapasu.pocetcervenychkaret%TYPE
,   v_pocet_goly_domaci IN vysledky_zapasu.pocetgolydomacitym%TYPE
,   v_pocet_goly_hoste IN vysledky_zapasu.pocetgolyhostetym%TYPE
);


-- Funkce pro kontrolu validace formátu výsledku
FUNCTION FN_JE_FORMAT_VYSLEDKU_VALIDNI 
(
    v_vysledek IN VARCHAR2
    
) RETURN NUMBER;

END PKG_VYSLEDKY_ZAPASU;

/
--------------------------------------------------------
--  DDL for Package PKG_ZAPASY
--------------------------------------------------------

  CREATE OR REPLACE EDITIONABLE PACKAGE "ST72870"."PKG_ZAPASY" AS 

-- Procedura pro vytvoøení nového zápasu
PROCEDURE SP_ADD_ZAPAS
(
    v_datum IN zapasy.datum%TYPE
,   v_id_soutez IN zapasy.idsoutez%TYPE
,   v_id_stav IN zapasy.stav_zapasu_idstav%TYPE
,   v_domaci_tym IN zapasy.domacitym%TYPE
,   v_hoste_tym IN zapasy.hostetym%TYPE
);


-- Procedura pro odebrání urèitého zápasu
PROCEDURE SP_ODEBER_ZAPAS
(
    v_id_zapas IN zapasy.idzapas%TYPE
);


-- Procedura pro editaci vybraného zápasu
PROCEDURE SP_UPDATE_ZAPAS
(
    v_id_zapas IN zapasy.idzapas%TYPE
,   v_datum IN zapasy.datum%TYPE
,   v_id_soutez IN zapasy.idsoutez%TYPE
,   v_id_stav IN zapasy.stav_zapasu_idstav%TYPE
,   v_domaci_tym IN zapasy.domacitym%TYPE
,   v_hoste_tym IN zapasy.hostetym%TYPE
);

------------------------------------------------------------------
-- Funkce – vrací textové shrnutí stavu všech zápasù
------------------------------------------------------------------
FUNCTION F_STAV_ZAPASU_SHRNUTI RETURN VARCHAR2;

END PKG_ZAPASY;

/
--------------------------------------------------------
--  DDL for Package Body PKG_BINARNI_OBSAH
--------------------------------------------------------

  CREATE OR REPLACE EDITIONABLE PACKAGE BODY "ST72870"."PKG_BINARNI_OBSAH" AS

    ------------------------------------------------------------------
    -- Pøidání nového binárního souboru
    ------------------------------------------------------------------
    PROCEDURE SP_ADD_OBSAH(
        v_nazev_souboru     IN BINARNI_OBSAH.NAZEVSOUBORU%TYPE,
        v_typ_souboru       IN BINARNI_OBSAH.TYPSOUBORU%TYPE,
        v_pripona_souboru   IN BINARNI_OBSAH.PRIPONASOUBORU%TYPE,
        v_obsah             IN BINARNI_OBSAH.OBSAH%TYPE,
        v_operace           IN BINARNI_OBSAH.OPERACE%TYPE,
        v_id_uzivatel       IN BINARNI_OBSAH.IDUZIVATELSKYUCET%TYPE
    )
    AS
    BEGIN
        INSERT INTO BINARNI_OBSAH (
            IDBINARNIOBSAH, NAZEVSOUBORU, TYPSOUBORU, PRIPONASOUBORU,
            OBSAH, DATUMNAHRANI, DATUMMODIFIKACE, OPERACE, IDUZIVATELSKYUCET
        )
        VALUES (
            SEKV_OBSAH.NEXTVAL,
            v_nazev_souboru,
            v_typ_souboru,
            v_pripona_souboru,
            v_obsah,
            SYSDATE,
            SYSDATE,
            v_operace,
            v_id_uzivatel
        );

        COMMIT; -- potvrzení operace
    EXCEPTION
        WHEN OTHERS THEN
            ROLLBACK;
            RAISE_APPLICATION_ERROR(-20001, 'Chyba pøi pøidávání binárního obsahu: ' || SQLERRM);
    END SP_ADD_OBSAH;

    ------------------------------------------------------------------
    -- Aktualizace existujícího binárního souboru podle ID
    ------------------------------------------------------------------
    PROCEDURE SP_UPDATE_OBSAH(
      v_id_obsah          IN BINARNI_OBSAH.IDBINARNIOBSAH%TYPE,
      v_obsah             IN BINARNI_OBSAH.OBSAH%TYPE,
      v_operace           IN BINARNI_OBSAH.OPERACE%TYPE,
      v_id_uzivatel       IN BINARNI_OBSAH.IDUZIVATELSKYUCET%TYPE
  )
  AS
  BEGIN
    -- Aktualizace dat podle ID
      UPDATE BINARNI_OBSAH
      SET 
          OBSAH = v_obsah,
          DATUMMODIFIKACE = SYSDATE,
          OPERACE = v_operace,
          IDUZIVATELSKYUCET = v_id_uzivatel
      WHERE IDBINARNIOBSAH = v_id_obsah;
     
     -- Kontrola, jestli existoval záznam
      IF SQL%ROWCOUNT = 0 THEN
          RAISE_APPLICATION_ERROR(-20002, 'Soubor s daným ID neexistuje!');
      END IF;

      COMMIT; -- uložení zmìn
  END SP_UPDATE_OBSAH;
    
    ------------------------------------------------------------------
    -- Pøejmenování souboru
    ------------------------------------------------------------------
      PROCEDURE SP_RENAME_OBSAH(
      v_id_obsah    IN BINARNI_OBSAH.IDBINARNIOBSAH%TYPE,
      v_novy_nazev  IN BINARNI_OBSAH.NAZEVSOUBORU%TYPE,
      v_id_uzivatel IN BINARNI_OBSAH.IDUZIVATELSKYUCET%TYPE
  )
  AS
  BEGIN
      UPDATE BINARNI_OBSAH
      SET 
          NAZEVSOUBORU = v_novy_nazev,
          DATUMMODIFIKACE = SYSDATE,
          OPERACE = 'uprava',
          IDUZIVATELSKYUCET = v_id_uzivatel  
      WHERE IDBINARNIOBSAH = v_id_obsah;

      IF SQL%ROWCOUNT = 0 THEN
          RAISE_APPLICATION_ERROR(-20020, 'Soubor s daným ID neexistuje!');
      END IF;

      COMMIT;
  END SP_RENAME_OBSAH;

    ------------------------------------------------------------------
    -- Smazání binárního souboru podle ID
    ------------------------------------------------------------------
    PROCEDURE SP_DELETE_OBSAH(
        v_id_obsah IN BINARNI_OBSAH.IDBINARNIOBSAH%TYPE
    )
    AS
    BEGIN
        -- Smazání podle ID
        DELETE FROM BINARNI_OBSAH
        WHERE IDBINARNIOBSAH = v_id_obsah;
        
        -- Pokud se nesmazal žádný øádek - ID není v DB
        IF SQL%ROWCOUNT = 0 THEN
            RAISE_APPLICATION_ERROR(-20005, 'Soubor s daným ID nebyl nalezen!');
        END IF;

        COMMIT;
    EXCEPTION
        WHEN NO_DATA_FOUND THEN
            ROLLBACK;
            RAISE_APPLICATION_ERROR(-20006, 'Soubor s daným ID neexistuje!');
        WHEN OTHERS THEN
            ROLLBACK;
            RAISE_APPLICATION_ERROR(-20007, 'Chyba pøi mazání souboru: ' || SQLERRM);
    END SP_DELETE_OBSAH;

END PKG_BINARNI_OBSAH;

/
--------------------------------------------------------
--  DDL for Package Body PKG_HRACI
--------------------------------------------------------

  CREATE OR REPLACE EDITIONABLE PACKAGE BODY "ST72870"."PKG_HRACI" AS

------------------------------------------------------------------
-- PØIDÁNÍ HRÁÈE
------------------------------------------------------------------
PROCEDURE SP_ADD_HRAC(
    v_rodne_cislo      IN CLENOVE_KLUBU.RODNE_CISLO%TYPE,
    v_jmeno            IN CLENOVE_KLUBU.JMENO%TYPE,
    v_prijmeni         IN CLENOVE_KLUBU.PRIJMENI%TYPE,
    v_telefonni_cislo  IN CLENOVE_KLUBU.TELEFONNICISLO%TYPE,
    v_id_pozice        IN HRACI.ID_POZICE%TYPE,
    v_pocet_golu       IN HRACI.POCETVSTRELENYCHGOLU%TYPE DEFAULT 0,
    v_pocet_zlute      IN HRACI.POCET_ZLUTYCH_KARET%TYPE DEFAULT 0,
    v_pocet_cervene    IN HRACI.POCET_CERVENYCH_KARET%TYPE DEFAULT 0,
    v_datum_opatreni   IN DISCIPLINARNI_OPATRENI.DATUMOPATRENI%TYPE DEFAULT NULL,
    v_delka_trestu     IN DISCIPLINARNI_OPATRENI.DELKATRESTU%TYPE DEFAULT NULL,
    v_duvod            IN DISCIPLINARNI_OPATRENI.DUVOD%TYPE DEFAULT NULL
)
AS
    v_id_clen     NUMBER;
    v_id_opatreni NUMBER;
BEGIN
    -- Generuj ID èlena
    SELECT SEKV_CLENKLUBU.NEXTVAL INTO v_id_clen FROM DUAL;

    -- Vložení do CLENOVE_KLUBU
    INSERT INTO CLENOVE_KLUBU (IDCLENKLUBU, RODNE_CISLO, JMENO, PRIJMENI, TELEFONNICISLO, TYPCLENA)
    VALUES (v_id_clen, v_rodne_cislo, v_jmeno, v_prijmeni, v_telefonni_cislo, 'Hrac');

    -- Pokud má disciplinární opatøení - vložit
    IF v_datum_opatreni IS NOT NULL THEN
        SELECT SEKV_OPATRENI.NEXTVAL INTO v_id_opatreni FROM DUAL;

        INSERT INTO DISCIPLINARNI_OPATRENI (IDOPATRENI, DATUMOPATRENI, DELKATRESTU, DUVOD)
        VALUES (v_id_opatreni, v_datum_opatreni, v_delka_trestu, v_duvod);
    ELSE
        v_id_opatreni := NULL;
    END IF;

    -- Vložení hráèe do HRACI
    INSERT INTO HRACI (
        IDCLENKLUBU, 
        ID_POZICE,
        POCETVSTRELENYCHGOLU,
        POCET_ZLUTYCH_KARET,
        POCET_CERVENYCH_KARET,
        IDOPATRENI
    ) VALUES (
        v_id_clen,
        v_id_pozice,     
        v_pocet_golu,
        v_pocet_zlute,
        v_pocet_cervene,
        v_id_opatreni
    );

    COMMIT;

EXCEPTION
    WHEN OTHERS THEN
        ROLLBACK;
        RAISE_APPLICATION_ERROR(-20010, 'Chyba pøi pøidávání hráèe: ' || SQLERRM);
END SP_ADD_HRAC;

------------------------------------------------------------------
-- UPDATE HRÁÈE
------------------------------------------------------------------
PROCEDURE SP_UPDATE_HRAC(
    v_rodne_cislo_puvodni IN CLENOVE_KLUBU.RODNE_CISLO%TYPE,
    v_rodne_cislo      IN CLENOVE_KLUBU.RODNE_CISLO%TYPE,
    v_jmeno            IN CLENOVE_KLUBU.JMENO%TYPE,
    v_prijmeni         IN CLENOVE_KLUBU.PRIJMENI%TYPE,
    v_telefonni_cislo  IN CLENOVE_KLUBU.TELEFONNICISLO%TYPE,
    v_id_pozice        IN HRACI.ID_POZICE%TYPE,
    v_pocet_golu       IN HRACI.POCETVSTRELENYCHGOLU%TYPE,
    v_pocet_zlute      IN HRACI.POCET_ZLUTYCH_KARET%TYPE,
    v_pocet_cervene    IN HRACI.POCET_CERVENYCH_KARET%TYPE,
    v_datum_opatreni   IN DISCIPLINARNI_OPATRENI.DATUMOPATRENI%TYPE,
    v_delka_trestu     IN DISCIPLINARNI_OPATRENI.DELKATRESTU%TYPE,
    v_duvod            IN DISCIPLINARNI_OPATRENI.DUVOD%TYPE
)
AS
    v_id_clen     NUMBER;
    v_id_opatreni NUMBER;
BEGIN
    -- Najdi ID èlena a ID opatøení
    SELECT c.IDCLENKLUBU, h.IDOPATRENI
      INTO v_id_clen, v_id_opatreni
      FROM CLENOVE_KLUBU c
      JOIN HRACI h ON c.IDCLENKLUBU = h.IDCLENKLUBU
     WHERE c.RODNE_CISLO = v_rodne_cislo_puvodni;

    -----------------------------
    -- DISCIPLINÁRNÍ OPATØENÍ
    -----------------------------
    IF v_datum_opatreni IS NOT NULL THEN

        IF v_id_opatreni IS NULL THEN
            -- Vytvoøit nové opatøení
            SELECT SEKV_OPATRENI.NEXTVAL INTO v_id_opatreni FROM DUAL;

            INSERT INTO DISCIPLINARNI_OPATRENI (IDOPATRENI, DATUMOPATRENI, DELKATRESTU, DUVOD)
            VALUES (v_id_opatreni, v_datum_opatreni, v_delka_trestu, v_duvod);

            UPDATE HRACI SET IDOPATRENI = v_id_opatreni WHERE IDCLENKLUBU = v_id_clen;

        ELSE
            -- Aktualizovat existující opatøení
            UPDATE DISCIPLINARNI_OPATRENI
               SET DATUMOPATRENI = v_datum_opatreni,
                   DELKATRESTU = v_delka_trestu,
                   DUVOD = v_duvod
             WHERE IDOPATRENI = v_id_opatreni;
        END IF;

    ELSE
        -- Žádné opatøení - smazat
        IF v_id_opatreni IS NOT NULL THEN
            UPDATE HRACI SET IDOPATRENI = NULL WHERE IDCLENKLUBU = v_id_clen;
            DELETE FROM DISCIPLINARNI_OPATRENI WHERE IDOPATRENI = v_id_opatreni;
        END IF;
    END IF;

    -- UPDATE CLENA
    UPDATE CLENOVE_KLUBU
       SET RODNE_CISLO = v_rodne_cislo,
           JMENO = v_jmeno,
           PRIJMENI = v_prijmeni,
           TELEFONNICISLO = v_telefonni_cislo
     WHERE IDCLENKLUBU = v_id_clen;

    -- UPDATE HRÁÈE
    UPDATE HRACI
       SET ID_POZICE = v_id_pozice,
           POCETVSTRELENYCHGOLU = v_pocet_golu,
           POCET_ZLUTYCH_KARET = v_pocet_zlute,
           POCET_CERVENYCH_KARET = v_pocet_cervene
     WHERE IDCLENKLUBU = v_id_clen;

    COMMIT;

EXCEPTION
    WHEN NO_DATA_FOUND THEN
        RAISE_APPLICATION_ERROR(-20011, 'Hráè nebyl nalezen');
    WHEN OTHERS THEN
        ROLLBACK;
        RAISE_APPLICATION_ERROR(-20012, 'Chyba UPDATE HRÁÈE: ' || SQLERRM);
END SP_UPDATE_HRAC;

-------------------------------
-- SMAZÁNÍ HRÁÈE
-------------------------------
PROCEDURE SP_ODEBER_HRACE(v_rodne_cislo IN CLENOVE_KLUBU.RODNE_CISLO%TYPE)
AS
    v_id_clen NUMBER;
    v_id_opatreni NUMBER;
BEGIN
    -- Najdeme hráèe podle rodného èísla
    SELECT c.IDCLENKLUBU, h.IDOPATRENI
      INTO v_id_clen, v_id_opatreni
      FROM CLENOVE_KLUBU c
      JOIN HRACI h ON c.IDCLENKLUBU = h.IDCLENKLUBU
     WHERE c.RODNE_CISLO = v_rodne_cislo;
    
    -- Pokud má opatøení - smažeme ho
    IF v_id_opatreni IS NOT NULL THEN
        UPDATE HRACI SET IDOPATRENI = NULL WHERE IDCLENKLUBU = v_id_clen;
        DELETE FROM DISCIPLINARNI_OPATRENI WHERE IDOPATRENI = v_id_opatreni;
    END IF;

    DELETE FROM HRACI WHERE IDCLENKLUBU = v_id_clen;
    DELETE FROM CLENOVE_KLUBU WHERE IDCLENKLUBU = v_id_clen;

    COMMIT;

EXCEPTION
    WHEN NO_DATA_FOUND THEN
        RAISE_APPLICATION_ERROR(-20013, 'Hráè nebyl nalezen');
    WHEN OTHERS THEN
        ROLLBACK;
        RAISE_APPLICATION_ERROR(-20014, 'Chyba pøi mazání hráèe: ' || SQLERRM);
END SP_ODEBER_HRACE;


------------------------------------------------------------------
-- FUNKCE — PRO CENTA GÓLÙ DLE POZICE (PØES ID)
------------------------------------------------------------------
FUNCTION F_GOLY_PROCENTA_POZICE(
    p_pozice IN HRACI.ID_POZICE%TYPE
) RETURN NUMBER
AS
    v_goly_pozice NUMBER := 0;
    v_goly_celkem NUMBER := 0;
BEGIN
    -- Celkový souèet gólù
    SELECT NVL(SUM(POCETVSTRELENYCHGOLU), 0)
      INTO v_goly_celkem
      FROM HRACI;
      
    ---- Góly hráèù konkrétní pozice
    SELECT NVL(SUM(POCETVSTRELENYCHGOLU), 0)
      INTO v_goly_pozice
      FROM HRACI
     WHERE ID_POZICE = p_pozice;
    
    -- pokud nikdo nedal gól - 0 %
    IF v_goly_celkem = 0 THEN
        RETURN 0;
    END IF;
    
    -- výpoèet procent
    RETURN ROUND((v_goly_pozice / v_goly_celkem) * 100, 2);

EXCEPTION
    WHEN OTHERS THEN
      RETURN 0;
END F_GOLY_PROCENTA_POZICE;

END PKG_HRACI;

/
--------------------------------------------------------
--  DDL for Package Body PKG_KONTRAKTY
--------------------------------------------------------

  CREATE OR REPLACE EDITIONABLE PACKAGE BODY "ST72870"."PKG_KONTRAKTY" AS

-- Procedura pro vytvoøení nového kontraktu
PROCEDURE SP_ADD_KONTRAKT
(
    v_id_clen IN kontrakty.idclenklubu%TYPE
,   v_datum_zacatku IN kontrakty.datumzacatku%TYPE
,   v_datum_konce IN kontrakty.datumkonce%TYPE
,   v_plat IN kontrakty.plat%TYPE
,   v_cislo_na_agenta IN kontrakty.cislonaagenta%TYPE
,   v_vystupni_klauzule IN kontrakty.vystupniklauzule%TYPE
)AS

    -- Definice vlastní výjimky pro nenalezení nadøazeného klíèe
    E_FOREIGN_KEY_NOT_FOUND EXCEPTION;

    -- Svázání vlastní výjimky s konkrétní chybou (ORA-02291)
    PRAGMA EXCEPTION_INIT(E_FOREIGN_KEY_NOT_FOUND, -2291);

BEGIN

    INSERT INTO kontrakty(idclenklubu, datumzacatku, datumkonce, plat, cislonaagenta, vystupniklauzule)
    VALUES(
        v_id_clen,
        v_datum_zacatku,
        v_datum_konce,
        v_plat,
        v_cislo_na_agenta,
        v_vystupni_klauzule
    );
    
    COMMIT;
    
    EXCEPTION
        -- Pokud se poruší UNIQUE CONSTRAINT, tak se vyvolá vlastní výjimka -20002 
        WHEN DUP_VAL_ON_INDEX THEN
            RAISE_APPLICATION_ERROR(-20002, 'Chyba: Hráè s ID "' || v_id_clen ||'" již v tabulce má platný kontrakt!');
        
        -- Pokud se nenajde nadøazený klíè v jiné tabulce, tak se vyvolá vlastní výjimka -20003
        WHEN E_FOREIGN_KEY_NOT_FOUND THEN
            RAISE_APPLICATION_ERROR(-20003, 'Chyba: Hráè s ID "' || v_id_clen || '" neexistuje (porušení FK constraint)!');
        
        WHEN OTHERS THEN
            ROLLBACK;
            RAISE;

END SP_ADD_KONTRAKT;


-- Procedura pro odebrání urèitého kontraktu
PROCEDURE SP_ODEBER_KONTRAKT
(
    v_id_clen IN kontrakty.idclenklubu%TYPE
)AS 
BEGIN

    DELETE FROM kontrakty
    WHERE idclenklubu = v_id_clen;
    
    -- Pokud se nenajde žádný kontrakt se zadaným ID, tak se vyvolá vlastní výjimka -20002 
    IF SQL%ROWCOUNT = 0 THEN
        RAISE_APPLICATION_ERROR(-20002, 'Chyba: Kontrakt s ID "' || v_id_clen || '" v tabulce neexistuje!');
    END IF;
    
    COMMIT;
    
    EXCEPTION       
        WHEN OTHERS THEN
            ROLLBACK;
            RAISE;
            
END SP_ODEBER_KONTRAKT;


-- Procedura pro editaci vybraného kontraktu
PROCEDURE SP_UPDATE_KONTRAKT
(
    v_id_clen IN kontrakty.idclenklubu%TYPE
,   v_datum_zacatku IN kontrakty.datumzacatku%TYPE
,   v_datum_konce IN kontrakty.datumkonce%TYPE
,   v_plat IN kontrakty.plat%TYPE
,   v_cislo_na_agenta IN kontrakty.cislonaagenta%TYPE
,   v_vystupni_klauzule IN kontrakty.vystupniklauzule%TYPE
)AS

BEGIN
    
    UPDATE kontrakty
    SET datumzacatku = v_datum_zacatku,
        datumkonce = v_datum_konce,
        plat = v_plat,
        cislonaagenta = v_cislo_na_agenta,
        vystupniklauzule = v_vystupni_klauzule
    WHERE idclenklubu = v_id_clen;
    
     -- Pokud se nenajde žádný kontrakt se zadaným ID, tak se vyvolá vlastní výjimka -20002 
    IF SQL%ROWCOUNT = 0 THEN
        RAISE_APPLICATION_ERROR(-20002, 'Chyba: Kontrakt s ID "' || v_id_clen || '" v tabulce neexistuje!');
    END IF;
    
    COMMIT;
    
    EXCEPTION
        -- Pokud se poruší UNIQUE CONSTRAINT, tak se vyvolá vlastní výjimka -20002 
        WHEN DUP_VAL_ON_INDEX THEN
        RAISE_APPLICATION_ERROR(-20002, 'Chyba: Hráè s ID "' || v_id_clen ||'" již v tabulce má platný kontrakt!');
        
        WHEN OTHERS THEN
        ROLLBACK;
        RAISE;

END SP_UPDATE_KONTRAKT;


-- Procedura pro kontrolu, zda není datum konce døíve než zaèátek
PROCEDURE SP_KONTROLA_DATUMU
(
    v_zacatek kontrakty.datumzacatku%TYPE
,   v_konec kontrakty.datumkonce%TYPE
)AS

BEGIN
    
    IF v_konec < v_zacatek THEN
        RAISE_APPLICATION_ERROR(-20001, 'Konec kontraktu nemùže být døíve než zaèátek!');
    END IF;
        
END SP_KONTROLA_DATUMU;


-- Procedura pro zvýšení platu hráèe o 5% pokud má minimálnì 3 sponzori
PROCEDURE SP_ZVYS_PLAT_HRACE
AS

BEGIN

    -- Implicitní kurzor pro nalezení hráèù, kteøí mají 3 nebo více sponzorù
    FOR hrac IN 
    (
        SELECT idclenklubu
        FROM sponzori_clenove
        GROUP BY idclenklubu
        HAVING COUNT(idsponzor) >= 3
    )
    
    -- Zvýšení platu všech hráèù, kteøí se našli pomocí implicitního kurzoru
    LOOP
    
        UPDATE kontrakty
        SET plat = plat * 1.05
        WHERE idclenklubu = hrac.idclenklubu;
        
    END LOOP;

END SP_ZVYS_PLAT_HRACE;


-- Procedura pro nalezení kontraktù, kterým konèí platnost za urèité èasové období (DEFAULTNÌ 30 dní)
PROCEDURE SP_KONTROLA_KONCICICH_KONTRAKTU 
(
    v_pocet_dni_do_konce_platnosti IN NUMBER
,   v_json_vystup OUT CLOB 
)
AS
    -- Promìnná pro deklaraci JSON pole
    v_json_array JSON_ARRAY_T := JSON_ARRAY_T();
    
    CURSOR c_koncici_kontrakty IS
        SELECT
            k.idclenklubu,
            ck.jmeno,
            ck.prijmeni,
            k.datumKonce
        FROM kontrakty k
        JOIN hraci h ON k.idclenklubu = h.idclenklubu
        JOIN clenove_klubu ck ON h.idclenklubu = ck.idclenklubu
        WHERE
            k.datumkonce IS NOT NULL AND k.datumkonce BETWEEN SYSDATE AND SYSDATE + v_pocet_dni_do_konce_platnosti
        ORDER BY
            k.datumkonce ASC;
            
    i_row c_koncici_kontrakty%ROWTYPE;

BEGIN
    -- Otevøení explicitního kurzoru
    OPEN c_koncici_kontrakty;

    -- Cyklické procházení záznamù v kurzoru
    LOOP
    
        FETCH c_koncici_kontrakty INTO i_row;
        EXIT WHEN c_koncici_kontrakty%NOTFOUND;

        -- Sestavení JSON objektu pro jeden kontrakt
        v_json_array.append(
            JSON_OBJECT
            (
                'IdHrace' VALUE i_row.idClenKlubu,
                'Jmeno' VALUE i_row.jmeno,
                'Prijmeni' VALUE i_row.prijmeni,
                'DatumUkonceni' VALUE TO_CHAR(i_row.datumKonce, 'DD.MM.YYYY'),
                'DnuDoKonce' VALUE TRUNC(i_row.datumKonce - SYSDATE)
            )
        );

    END LOOP;

    -- Uzavøení kurzoru
    CLOSE c_koncici_kontrakty;

    -- Vrácení JSON pole jako CLOB do výstupního parametru
    v_json_vystup := v_json_array.to_clob();

EXCEPTION
    WHEN OTHERS THEN
            -- Uzavøení kurzoru v pøípadì chyby
            IF c_koncici_kontrakty%ISOPEN THEN
                CLOSE c_koncici_kontrakty;
            END IF;
            
            RAISE;
END SP_KONTROLA_KONCICICH_KONTRAKTU;

END PKG_KONTRAKTY;

/
--------------------------------------------------------
--  DDL for Package Body PKG_LOG_TABLE
--------------------------------------------------------

  CREATE OR REPLACE EDITIONABLE PACKAGE BODY "ST72870"."PKG_LOG_TABLE" AS

-- Procedura pro zapisování zmìn v datech v jednotlivých tabulkách
PROCEDURE SP_ADD_RECORD_LOG_TABLE
(
    v_operace IN log_table.operace%TYPE
,   v_cas IN log_table.cas%TYPE
,   v_uzivatel IN log_table.uzivatel%TYPE
,   v_tabulka IN log_table.tabulka%TYPE
) AS
BEGIN
    
    INSERT INTO log_table (operace, cas, uzivatel, tabulka)
    VALUES (
        v_operace,
        v_cas,
        v_uzivatel,
        UPPER(v_tabulka)
    );
    
    EXCEPTION
        WHEN OTHERS THEN
            RAISE;
    
END SP_ADD_RECORD_LOG_TABLE;

END PKG_LOG_TABLE;

/
--------------------------------------------------------
--  DDL for Package Body PKG_REGISTRACE
--------------------------------------------------------

  CREATE OR REPLACE EDITIONABLE PACKAGE BODY "ST72870"."PKG_REGISTRACE" AS

    ----------------------------------------------------------------------
    -- Procedura: SP_ADD_UZIVATEL
    ----------------------------------------------------------------------
    PROCEDURE SP_ADD_UZIVATEL (
        v_uzivatelske_jmeno   IN UZIVATELSKE_UCTY.UZIVATELSKEJMENO%TYPE,
        v_heslo               IN UZIVATELSKE_UCTY.HESLO%TYPE,
        v_posledni_prihlaseni IN UZIVATELSKE_UCTY.POSLEDNIPRIHLASENI%TYPE,
        v_idrole              IN UZIVATELSKE_UCTY.IDROLE%TYPE,
        v_salt                IN UZIVATELSKE_UCTY.SALT%TYPE,
        v_email               IN UZIVATELSKE_UCTY.EMAIL%TYPE
    ) AS
    BEGIN
        INSERT INTO UZIVATELSKE_UCTY (
            IDUZIVATELSKYUCET,
            UZIVATELSKEJMENO,
            HESLO,
            POSLEDNIPRIHLASENI,
            IDROLE,
            SALT,
            EMAIL
        )
        VALUES (
            SEKV_UZIVATELSKE_UCTY.NEXTVAL,
            v_uzivatelske_jmeno,
            v_heslo,
            v_posledni_prihlaseni,
            v_idrole,
            v_salt,
            v_email
        );

        COMMIT;

    EXCEPTION
        WHEN DUP_VAL_ON_INDEX THEN
            RAISE_APPLICATION_ERROR(-20003, 'Duplicitní uživatelské jméno nebo e-mail');
        WHEN OTHERS THEN
            ROLLBACK;
            RAISE_APPLICATION_ERROR(-20001, 'Chyba pøi pøidávání uživatele: ' || SQLERRM);
    END SP_ADD_UZIVATEL;

    ----------------------------------------------------------------------
    -- Procedura: SP_DELETE_UZIVATEL
    ----------------------------------------------------------------------
    PROCEDURE SP_DELETE_UZIVATEL (
        v_uzivatelske_jmeno IN UZIVATELSKE_UCTY.UZIVATELSKEJMENO%TYPE
    ) AS
    BEGIN
        DELETE FROM UZIVATELSKE_UCTY
        WHERE UZIVATELSKEJMENO = v_uzivatelske_jmeno;

        IF SQL%ROWCOUNT = 0 THEN
            RAISE_APPLICATION_ERROR(-20004, 'Uživatel neexistuje');
        END IF;

        COMMIT;

    EXCEPTION
        WHEN OTHERS THEN
            ROLLBACK;
            RAISE_APPLICATION_ERROR(-20002, 'Chyba pøi mazání uživatele: ' || SQLERRM);
    END SP_DELETE_UZIVATEL;

    ----------------------------------------------------------------------
    -- Procedura: SP_UPDATE_UZIVATEL
    ----------------------------------------------------------------------
    PROCEDURE SP_UPDATE_UZIVATEL (
        v_stare_jmeno        IN UZIVATELSKE_UCTY.UZIVATELSKEJMENO%TYPE,
        v_nove_jmeno         IN UZIVATELSKE_UCTY.UZIVATELSKEJMENO%TYPE,
        v_email              IN UZIVATELSKE_UCTY.EMAIL%TYPE,
        v_heslo              IN UZIVATELSKE_UCTY.HESLO%TYPE,
        v_salt               IN UZIVATELSKE_UCTY.SALT%TYPE,
        v_idrole             IN UZIVATELSKE_UCTY.IDROLE%TYPE
    ) AS
    BEGIN
        UPDATE UZIVATELSKE_UCTY
        SET UZIVATELSKEJMENO = v_nove_jmeno,
            EMAIL            = v_email,
            HESLO            = v_heslo,
            SALT             = v_salt,
            IDROLE           = v_idrole
        WHERE UZIVATELSKEJMENO = v_stare_jmeno;

        IF SQL%ROWCOUNT = 0 THEN
            RAISE_APPLICATION_ERROR(-20005, 'Uživatel nebyl nalezen');
        END IF;

        COMMIT;

    EXCEPTION
        WHEN DUP_VAL_ON_INDEX THEN
            RAISE_APPLICATION_ERROR(-20007, 'Uživatelské jméno nebo e-mail již existuje');
        WHEN OTHERS THEN
            ROLLBACK;
            RAISE_APPLICATION_ERROR(-20006, 'Chyba pøi aktualizaci uživatele: ' || SQLERRM);
    END SP_UPDATE_UZIVATEL;

    ----------------------------------------------------------------------
    -- Procedura: SP_NASTAV_CLENA
    ----------------------------------------------------------------------
    PROCEDURE SP_NASTAV_CLENA(
    v_uzivatelske_jmeno IN UZIVATELSKE_UCTY.UZIVATELSKEJMENO%TYPE,
    v_id_clen           IN UZIVATELSKE_UCTY.CLEN_KLUBU_IDCLENKLUBU%TYPE
)   AS
    BEGIN
    UPDATE UZIVATELSKE_UCTY
    SET CLEN_KLUBU_IDCLENKLUBU = v_id_clen
    WHERE UZIVATELSKEJMENO = v_uzivatelske_jmeno;

    IF SQL%ROWCOUNT = 0 THEN
        RAISE_APPLICATION_ERROR(-20050, 'Uživatel neexistuje');
    END IF;

    COMMIT;
END SP_NASTAV_CLENA;

    ----------------------------------------------------------------------
    -- Procedura: SP_UPDATE_POSLEDNI_PRIHLASENI
    ----------------------------------------------------------------------
    PROCEDURE SP_UPDATE_POSLEDNI_PRIHLASENI (
    v_uzivatelske_jmeno IN UZIVATELSKE_UCTY.UZIVATELSKEJMENO%TYPE
) AS
BEGIN
    UPDATE UZIVATELSKE_UCTY
    SET POSLEDNIPRIHLASENI = SYSTIMESTAMP
    WHERE UZIVATELSKEJMENO = v_uzivatelske_jmeno;

    IF SQL%ROWCOUNT = 0 THEN
        RAISE_APPLICATION_ERROR(-20008, 'Uživatel neexistuje');
    END IF;

    COMMIT;

EXCEPTION
    WHEN OTHERS THEN
        ROLLBACK;
        RAISE_APPLICATION_ERROR(-20009, 'Chyba pøi aktualizaci posledního pøihlášení: ' || SQLERRM);
END SP_UPDATE_POSLEDNI_PRIHLASENI;


END PKG_REGISTRACE;

/
--------------------------------------------------------
--  DDL for Package Body PKG_SOUTEZE
--------------------------------------------------------

  CREATE OR REPLACE EDITIONABLE PACKAGE BODY "ST72870"."PKG_SOUTEZE" AS

-- Procedura pro vytvoøení nové soutìže
PROCEDURE SP_ADD_SOUTEZ
(
    v_id_typ_souteze IN souteze.idtypsouteze%TYPE
,   v_datum_zacatek IN souteze.startdatum%TYPE
,   v_datum_konec IN souteze.konecdatum%TYPE
)AS

    -- Definice vlastní výjimky pro nenalezení nadøazeného klíèe
    E_FOREIGN_KEY_NOT_FOUND EXCEPTION;

    -- Svázání vlastní výjimky s konkrétní chybou (ORA-02291)
    PRAGMA EXCEPTION_INIT(E_FOREIGN_KEY_NOT_FOUND, -2291);

BEGIN

    INSERT INTO souteze (startdatum, konecdatum, idtypsouteze)
    VALUES (v_datum_zacatek, v_datum_konec, v_id_typ_souteze);
    
    COMMIT;
    
    EXCEPTION
    
        -- Pokud se nenajde nadøazený klíè v jiné tabulce, tak se vyvolá vlastní výjimka -20002 
        WHEN E_FOREIGN_KEY_NOT_FOUND THEN
            RAISE_APPLICATION_ERROR(-20002, 'Chyba: Typ soutìže s ID "' || v_id_typ_souteze || '" neexistuje (porušení FK constraint)!');

        WHEN OTHERS THEN
            ROLLBACK;
            RAISE;

END SP_ADD_SOUTEZ;


-- Procedura pro odebrání urèité soutìže
PROCEDURE SP_ODEBER_SOUTEZ 
(
    v_id_soutez IN souteze.idsoutez%TYPE
)AS 
BEGIN

    -- Smazání výsledkù zápasù, které chceme smazat
    DELETE FROM vysledky_zapasu
    WHERE idzapas IN 
    (
        SELECT idzapas FROM zapasy WHERE idsoutez = v_id_soutez
    );
    
    -- Smazání všech zápasù soutìže, kterou chceme smazat
    DELETE FROM zapasy
    WHERE idsoutez = v_id_soutez;
    
    -- Smazání urèité soutìže
    DELETE FROM souteze
    WHERE idsoutez = v_id_soutez;
    
    -- Pokud se nenajde žádná soutìž se zadaným ID, tak se vyvolá vlastní výjimka -20002
    IF SQL%ROWCOUNT = 0 THEN
        RAISE_APPLICATION_ERROR(-20002, 'Chyba: Soutìž s ID "' || v_id_soutez || '" v tabulce neexistuje!');
    END IF;
    
    COMMIT;
    
    EXCEPTION       
        WHEN OTHERS THEN
            ROLLBACK;
            RAISE;
            
END SP_ODEBER_SOUTEZ;


-- Procedura pro editaci vybrané soutìže
PROCEDURE SP_UPDATE_SOUTEZ
(
    v_id_soutez IN souteze.idsoutez%TYPE
,   v_id_typ_souteze IN souteze.idtypsouteze%TYPE
,   v_datum_zacatek IN souteze.startdatum%TYPE
,   v_datum_konec IN souteze.konecdatum%TYPE
)AS

BEGIN
    
    UPDATE souteze
    SET idtypsouteze = v_id_typ_souteze,
        konecdatum = v_datum_konec,
        startdatum = v_datum_zacatek
    WHERE idsoutez = v_id_soutez;
    
     -- Pokud se nenajde žádná soutìž se zadaným ID, tak se vyvolá vlastní výjimka -20002 
    IF SQL%ROWCOUNT = 0 THEN
        RAISE_APPLICATION_ERROR(-20002, 'Chyba: Soutìž s ID "' || v_id_soutez || '" v tabulce neexistuje!');
    END IF;
    
    COMMIT;
    
    EXCEPTION

        WHEN OTHERS THEN
        ROLLBACK;
        RAISE;

END SP_UPDATE_SOUTEZ;


-- Procedura pro kontrolu, zda není datum konce døíve než zaèátek
PROCEDURE SP_KONTROLA_DATUMU
(
    v_zacatek souteze.startdatum%TYPE
,   v_konec souteze.konecdatum%TYPE
)AS

BEGIN
    
    IF v_konec < v_zacatek THEN
        RAISE_APPLICATION_ERROR(-20001, 'Konec soutìže nemùže být døíve než zaèátek!');
    END IF;
        
END SP_KONTROLA_DATUMU;


-- Procedura pro výpis hierarchie soutìží pomocí hierarchického dotazu
PROCEDURE SP_VYPIS_HIERARCHII_SOUTEZI
(
    v_cursor OUT SYS_REFCURSOR
) AS

BEGIN

    OPEN v_cursor FOR
        SELECT 
            LEVEL AS uroven,
            LPAD(' ', (LEVEL - 1) * 5) ||
               CASE 
                    WHEN vsechny.typ = 'S' THEN CHR(10) || 'SOUTÌŽ: ' || vsechny.nazev
                    WHEN vsechny.typ = 'Z' THEN 'ZÁPAS: ' || vsechny.nazev
                    WHEN vsechny.typ = 'V' THEN 'VÝSLEDEK: ' || vsechny.nazev
               END AS text_radku
        FROM (
            -- Úroveò 1 - Soutìže
            SELECT s.idSoutez AS id,
                   NULL AS idNadrazeneho,
                   'S' AS typ,
                   'Typ= ' ||  typsout.nazevsouteze
                      || ', Start= ' || TO_CHAR(s.startDatum,'DD.MM.YYYY')
                      || ', Konec= ' || TO_CHAR(s.konecDatum,'DD.MM.YYYY') AS nazev
            FROM SOUTEZE s
            JOIN typ_soutez typsout ON typsout.idtypsouteze = s.idtypsouteze  

            UNION ALL

            -- Úroveò 2 – Zápasy
            SELECT z.idZapas + 10000 AS id,             -- Nové jedineèné ID Zápasu (ID + 10000)
                   z.idSoutez AS idNadrazeneho,
                   'Z' AS typ,
                   'Tým D= ' || z.domacitym
                   || ', Tým H= ' || z.hostetym
                   || ', Datum= ' || TO_CHAR(z.datum,'DD.MM.YYYY') 
                   || ', Stav= ' || stav.stavzapasu AS nazev
            FROM ZAPASY z
            JOIN stav_zapasu stav ON stav.idstav = z.stav_zapasu_idstav

            UNION ALL

            -- Úroveò 3 – Výsledky
            SELECT vz.idZapas + 20000 AS id,            -- Nové jedineèné ID Výsledku (ID + 20000)
                   vz.idZapas + 10000 AS idNadrazeneho, -- Rodiè je Zápas (ID + 10000)
                   'V' AS typ,
                   'Výsledek: ' || vz.vysledek
                      || ', Góly D= ' || vz.pocetGolyDomaciTym
                      || ', Góly H= ' || vz.pocetGolyHosteTym AS nazev
            FROM VYSLEDKY_ZAPASU vz
        ) vsechny
        
        START WITH vsechny.idNadrazeneho IS NULL
        CONNECT BY NOCYCLE PRIOR vsechny.id = vsechny.idNadrazeneho
        ORDER SIBLINGS BY vsechny.typ;
        
    EXCEPTION
        WHEN OTHERS THEN
            RAISE;
            
END SP_VYPIS_HIERARCHII_SOUTEZI;

END PKG_SOUTEZE;

/
--------------------------------------------------------
--  DDL for Package Body PKG_SPONZORI
--------------------------------------------------------

  CREATE OR REPLACE EDITIONABLE PACKAGE BODY "ST72870"."PKG_SPONZORI" AS

-- Procedura pro vytvoøení nového sponzora
PROCEDURE SP_ADD_SPONZOR 
(
    v_jmeno IN sponzori.jmeno%TYPE
,   v_sponzorovana_castka IN sponzori.sponzorovanacastka%TYPE
) AS 
BEGIN

    INSERT INTO sponzori (jmeno, sponzorovanacastka)
    VALUES(v_jmeno, v_sponzorovana_castka);
   
    COMMIT;
    
    EXCEPTION
        -- Pokud se poruší UNIQUE CONSTRAINT, tak se vyvolá vlastní výjimka -20002 
        WHEN DUP_VAL_ON_INDEX THEN
        RAISE_APPLICATION_ERROR(-20002, 'Chyba: Sponzor se jménem "' || v_jmeno || '" již v tabulce existuje!');
        
        WHEN OTHERS THEN
            ROLLBACK;
            RAISE;
            
END SP_ADD_SPONZOR;


-- Procedura pro odebrání urèitého sponzora
PROCEDURE SP_ODEBER_SPONZOR 
(
    v_id_sponzor IN sponzori.idsponzor%TYPE
) AS 
BEGIN

    DELETE FROM sponzori
    WHERE idsponzor = v_id_sponzor;
    
    -- Pokud se nenajde žádný sponzor se zadaným ID, tak se vyvolá vlastní výjimka -20002 
    IF SQL%ROWCOUNT = 0 THEN
        RAISE_APPLICATION_ERROR(-20002, 'Chyba: Sponzor s ID "' || v_id_sponzor || '" v tabulce neexistuje!');
    END IF;
    
    COMMIT;
    
    EXCEPTION       
        WHEN OTHERS THEN
            ROLLBACK;
            RAISE;
            
END SP_ODEBER_SPONZOR;


-- Procedura pro editaci vybraného sponzora
PROCEDURE SP_UPDATE_SPONZOR 
(
    v_id_sponzor IN sponzori.idsponzor%TYPE
,   v_jmeno IN sponzori.jmeno%TYPE
,   v_sponzorovana_castka IN sponzori.sponzorovanacastka%TYPE
) AS 
BEGIN

    UPDATE sponzori
    SET jmeno = v_jmeno,
        sponzorovanacastka = v_sponzorovana_castka
    WHERE idsponzor = v_id_sponzor;
    
    -- Pokud se nenajde žádný sponzor se zadaným ID, tak se vyvolá vlastní výjimka -20002 
    IF SQL%ROWCOUNT = 0 THEN
        RAISE_APPLICATION_ERROR(-20002, 'Chyba: Sponzor s ID "' || v_id_sponzor || '" v tabulce neexistuje!');
    END IF;
    
    COMMIT;
    
    EXCEPTION       
        -- Pokud se poruší UNIQUE CONSTRAINT, tak se vyvolá vlastní výjimka -20003
        WHEN DUP_VAL_ON_INDEX THEN
        RAISE_APPLICATION_ERROR(-20003, 'Chyba: Sponzor se jménem "' || v_jmeno || '" již v tabulce existuje!');
        
        WHEN OTHERS THEN
            ROLLBACK;
            RAISE;
            
END SP_UPDATE_SPONZOR;

END PKG_SPONZORI;

/
--------------------------------------------------------
--  DDL for Package Body PKG_SPONZORI_CLENOVE
--------------------------------------------------------

  CREATE OR REPLACE EDITIONABLE PACKAGE BODY "ST72870"."PKG_SPONZORI_CLENOVE" AS

-- Procedura pro vytvoøení nové vazby SPONZORI-CLENOVE
PROCEDURE SP_ADD_SPONZORI_CLENOVE 
(
  v_id_sponzor IN sponzori_clenove.idsponzor%TYPE
, v_id_clen IN sponzori_clenove.idclenklubu%TYPE
) AS    

    v_pocet NUMBER;
    
BEGIN
  
    -- Zkontrolování, jestli daná vazba už v tabulce existuje
    SELECT COUNT(*) INTO v_pocet
    FROM sponzori_clenove
    WHERE idclenklubu = v_id_clen
      AND idsponzor = v_id_sponzor;
      
    -- Pokud daná vazba už existuje, tak se vyvolá vlastní výjimka -20001
    IF v_pocet > 0 THEN
        RAISE_APPLICATION_ERROR(-20001,'Chyba: Vazba mezi èlenem klubu ID:"' || v_id_clen || '" a sponzorem ID:"' || v_id_sponzor || '" již existuje!');
    END IF;
    
    INSERT INTO sponzori_clenove(idsponzor, idclenklubu)
    VALUES(v_id_sponzor, v_id_clen);
    COMMIT;

EXCEPTION
    WHEN OTHERS THEN
        ROLLBACK;
        RAISE;
        
END SP_ADD_SPONZORI_CLENOVE;


-- Procedura pro odebrání urèité vazby SPONZORI-CLENOVE
PROCEDURE SP_ODEBER_SPONZORI_CLENOVE 
(
  v_id_sponzor IN sponzori_clenove.idsponzor%TYPE
, v_id_clen IN sponzori_clenove.idclenklubu%TYPE
) AS    
BEGIN
  
    DELETE FROM sponzori_clenove
    WHERE idsponzor = v_id_sponzor AND idclenklubu = v_id_clen;
    
    COMMIT;

EXCEPTION
    WHEN OTHERS THEN
        ROLLBACK;
        RAISE;
        
END SP_ODEBER_SPONZORI_CLENOVE;


-- Procedura pro odebrání všech vazeb SPONZORI-CLENOVE urèitého sponzora
PROCEDURE SP_ODEBER_VSECHNY_SPONZORI_CLENOVE 
(
  v_id_sponzor IN sponzori_clenove.idsponzor%TYPE
) AS    
BEGIN
  
    DELETE FROM sponzori_clenove
    WHERE idsponzor = v_id_sponzor;
    
    COMMIT;

EXCEPTION
    WHEN OTHERS THEN
        ROLLBACK;
        RAISE;
        
END SP_ODEBER_VSECHNY_SPONZORI_CLENOVE;

END PKG_SPONZORI_CLENOVE;

/
--------------------------------------------------------
--  DDL for Package Body PKG_SPONZORI_SOUTEZE
--------------------------------------------------------

  CREATE OR REPLACE EDITIONABLE PACKAGE BODY "ST72870"."PKG_SPONZORI_SOUTEZE" AS

-- Procedura pro vytvoøení nové vazby SPONZORI-SOUTEZE
PROCEDURE SP_ADD_SPONZORI_SOUTEZE 
(
    v_id_sponzor IN sponzori_souteze.idsponzor%TYPE
,   v_id_soutez IN sponzori_souteze.idsoutez%TYPE
) AS 

    v_pocet NUMBER;
    
BEGIN
  
    -- Zkontrolování, jestli daná vazba už v tabulce existuje
    SELECT COUNT(*) INTO v_pocet
    FROM sponzori_souteze
    WHERE idsoutez = v_id_soutez
      AND idsponzor = v_id_sponzor;
      
    -- Pokud daná vazba už existuje, tak se vyvolá vlastní výjimka -20001
    IF v_pocet > 0 THEN
        RAISE_APPLICATION_ERROR(-20001,'Chyba: Vazba mezi soutìží ID:"' || v_id_soutez || '" a sponzorem ID:"' || v_id_sponzor || '" již existuje!');
    END IF;
    
    INSERT INTO sponzori_souteze(idsponzor, idsoutez)
    VALUES(v_id_sponzor, v_id_soutez);
    COMMIT;

EXCEPTION
    WHEN OTHERS THEN
        ROLLBACK;
        RAISE;
  
END SP_ADD_SPONZORI_SOUTEZE;


-- Procedura pro odebrání urèité vazby SPONZORI-SOUTEZE
PROCEDURE SP_ODEBER_SPONZORI_SOUTEZE 
(
    v_id_sponzor IN sponzori_souteze.idsponzor%TYPE
,   v_id_soutez IN sponzori_souteze.idsoutez%TYPE
) AS 
BEGIN
  
    DELETE FROM sponzori_souteze
    WHERE idsponzor = v_id_sponzor AND idsoutez = v_id_soutez;
    
    COMMIT;

EXCEPTION
    WHEN OTHERS THEN
        ROLLBACK;
        RAISE;
  
END SP_ODEBER_SPONZORI_SOUTEZE;


-- Procedura pro odebrání všech vazeb SPONZORI-SOUTEZE urèitého sponzora
PROCEDURE SP_ODEBER_VSECHNY_SPONZORI_SOUTEZE
(
    v_id_sponzor IN sponzori_souteze.idsponzor%TYPE
) AS 
BEGIN
  
    DELETE FROM sponzori_souteze
    WHERE idsponzor = v_id_sponzor;
    
    COMMIT;

EXCEPTION
    WHEN OTHERS THEN
        ROLLBACK;
        RAISE;
  
END SP_ODEBER_VSECHNY_SPONZORI_SOUTEZE;

END PKG_SPONZORI_SOUTEZE;

/
--------------------------------------------------------
--  DDL for Package Body PKG_TRENERI
--------------------------------------------------------

  CREATE OR REPLACE EDITIONABLE PACKAGE BODY "ST72870"."PKG_TRENERI" AS

    ------------------------------------------------------------------
    -- Pøidá nového trenéra do databáze
    ------------------------------------------------------------------
    PROCEDURE SP_ADD_TRENERI(
        v_rodne_cislo       IN CLENOVE_KLUBU.RODNE_CISLO%TYPE,
        v_jmeno             IN CLENOVE_KLUBU.JMENO%TYPE,
        v_prijmeni          IN CLENOVE_KLUBU.PRIJMENI%TYPE,
        v_telefonni_cislo   IN CLENOVE_KLUBU.TELEFONNICISLO%TYPE,
        v_trenerska_licence IN TRENERI.TRENERSKALICENCE%TYPE,
        v_specializace      IN TRENERI.SPECIALIZACE%TYPE DEFAULT NULL,
        v_pocet_let_praxe   IN TRENERI.POCETLETPRAXE%TYPE
    ) AS
        v_id_clen_klubu NUMBER;
    BEGIN
        -- Získání nového ID èlena
        SELECT SEKV_CLENKLUBU.NEXTVAL INTO v_id_clen_klubu FROM DUAL;

        -- Vložení do tabulky CLENOVE_KLUBU
        INSERT INTO CLENOVE_KLUBU (
            IDCLENKLUBU, RODNE_CISLO, JMENO, PRIJMENI, TYPCLENA, TELEFONNICISLO
        ) VALUES (
            v_id_clen_klubu, v_rodne_cislo, v_jmeno, v_prijmeni, 'Trener', v_telefonni_cislo
        );

        -- Vložení do tabulky TRENERI
        INSERT INTO TRENERI (
            IDCLENKLUBU, TRENERSKALICENCE, SPECIALIZACE, POCETLETPRAXE
        ) VALUES (
            v_id_clen_klubu, v_trenerska_licence, v_specializace, v_pocet_let_praxe
        );

        COMMIT;

    EXCEPTION
        WHEN DUP_VAL_ON_INDEX THEN
            ROLLBACK;
            RAISE_APPLICATION_ERROR(-20001, 'Èlen s tímto rodným èíslem již existuje!');
        WHEN OTHERS THEN
            ROLLBACK;
            RAISE_APPLICATION_ERROR(-20002, 'Neoèekávaná chyba pøi pøidávání trenéra: ' || SQLERRM);
    END SP_ADD_TRENERI;

    ------------------------------------------------------------------
    -- Aktualizuje údaje o trenérovi
    ------------------------------------------------------------------
   PROCEDURE SP_UPDATE_TRENERI(
    v_rodne_cislo_puvodni IN CLENOVE_KLUBU.RODNE_CISLO%TYPE,
    v_rodne_cislo       IN CLENOVE_KLUBU.RODNE_CISLO%TYPE,
    v_jmeno             IN CLENOVE_KLUBU.JMENO%TYPE,
    v_prijmeni          IN CLENOVE_KLUBU.PRIJMENI%TYPE,
    v_telefonni_cislo   IN CLENOVE_KLUBU.TELEFONNICISLO%TYPE,
    v_trenerska_licence IN TRENERI.TRENERSKALICENCE%TYPE,
    v_specializace      IN TRENERI.SPECIALIZACE%TYPE,
    v_pocet_let_praxe   IN TRENERI.POCETLETPRAXE%TYPE
) AS
    v_id_clen NUMBER;
BEGIN
    -- Najdeme ID podle pùvodního rodného èísla
    SELECT IDCLENKLUBU INTO v_id_clen 
    FROM CLENOVE_KLUBU 
    WHERE RODNE_CISLO = v_rodne_cislo_puvodni;

    -- Aktualizujeme CLENOVE_KLUBU nebo i rodné èíslo
    UPDATE CLENOVE_KLUBU
       SET RODNE_CISLO     = v_rodne_cislo,
           JMENO           = v_jmeno,
           PRIJMENI        = v_prijmeni,
           TELEFONNICISLO  = v_telefonni_cislo
     WHERE IDCLENKLUBU = v_id_clen;

    -- Aktualizace TRENERI
    UPDATE TRENERI
       SET TRENERSKALICENCE = v_trenerska_licence,
           SPECIALIZACE     = v_specializace,
           POCETLETPRAXE    = v_pocet_let_praxe
     WHERE IDCLENKLUBU = v_id_clen;

    COMMIT;

EXCEPTION
    WHEN NO_DATA_FOUND THEN
        ROLLBACK;
        RAISE_APPLICATION_ERROR(-20003, 'Pùvodní rodné èíslo nebylo nalezeno!');
    WHEN DUP_VAL_ON_INDEX THEN
        ROLLBACK;
        RAISE_APPLICATION_ERROR(-20007, 'Nové rodné èíslo je již použité!');
    WHEN OTHERS THEN
        ROLLBACK;
        RAISE_APPLICATION_ERROR(-20004, 'Chyba pøi aktualizaci trenéra: ' || SQLERRM);
        
END SP_UPDATE_TRENERI;

    ------------------------------------------------------------------
    -- Odstraní trenéra podle rodného èísla
    ------------------------------------------------------------------
    PROCEDURE SP_ODEBER_TRENERI(
        v_rodne_cislo IN CLENOVE_KLUBU.RODNE_CISLO%TYPE
    ) AS
        v_id_clen NUMBER;
    BEGIN
        -- Zjištìní ID trenéra
        SELECT IDCLENKLUBU INTO v_id_clen 
        FROM CLENOVE_KLUBU 
        WHERE RODNE_CISLO = v_rodne_cislo;

        -- Smazání všech vazeb a záznamù
        DELETE FROM SPONZORI_CLENOVE WHERE IDCLENKLUBU = v_id_clen;
        DELETE FROM TRENINKY WHERE IDCLENKLUBU = v_id_clen;
        DELETE FROM TRENERI WHERE IDCLENKLUBU = v_id_clen;
        DELETE FROM CLENOVE_KLUBU WHERE IDCLENKLUBU = v_id_clen;

        COMMIT;

    EXCEPTION
        WHEN NO_DATA_FOUND THEN
            ROLLBACK;
            RAISE_APPLICATION_ERROR(-20005, 'Trenér s daným rodným èíslem neexistuje!');
        WHEN OTHERS THEN
            ROLLBACK;
            RAISE_APPLICATION_ERROR(-20006, 'Chyba pøi odstraòování trenéra: ' || SQLERRM);
    END SP_ODEBER_TRENERI;
    
    
    ------------------------------------------------------------------
    -- Funkce: vrací BLOB s TOP 3 trenéry podle praxe
    ------------------------------------------------------------------
   FUNCTION F_TOP3_TRENERI_BLOB
RETURN BLOB
AS
    v_blob   BLOB;

    -- Pomocná RAW promìnná pro zápis textu do BLOBu
    v_raw    RAW(32767);
BEGIN

    DBMS_LOB.CREATETEMPORARY(v_blob, TRUE);

    --   pøevádíme text do RAW (binární podoby), aby jej bylo možné
    --   zapsat do BLOBu
    v_raw := UTL_RAW.CAST_TO_RAW(
                'TOP 3 trenéøi podle praxe' || CHR(10) ||
                '---------------------------' || CHR(10) ||
                'Generováno: ' || TO_CHAR(SYSDATE, 'DD.MM.YYYY HH24:MI') || CHR(10) ||
                CHR(10)
             );

    -- Zápis hlavièky do BLOBu
    DBMS_LOB.WRITEAPPEND(v_blob, UTL_RAW.LENGTH(v_raw), v_raw);

    -- funkce ROW_NUMBER()
    --    - seznam trenérù, seøadí podle poètu let praxe a pøiøadí poøadí (1–3)
    --    - následnì se do BLOBu zapíší jednotlivé øádky

    DECLARE
        pocet NUMBER := 0;   -- Poèet trenérù
    BEGIN
        FOR t IN (
            SELECT jmeno, prijmeni, pocetletpraxe
            FROM (
                SELECT 
                    ck.jmeno,
                    ck.prijmeni,
                    tr.pocetletpraxe,

                    -- ROW_NUMBER = poøadí trenérù dle praxe
                    ROW_NUMBER() OVER (ORDER BY tr.pocetletpraxe DESC) AS rn

                FROM TRENERI tr
                JOIN CLENOVE_KLUBU ck ON ck.idclenklubu = tr.idclenklubu
            )
            WHERE rn <= 3        -- vezmeme jen TOP 3
            ORDER BY rn
        )
        LOOP
            pocet := pocet + 1;

            -- Pøipravíme textový øádek pro daného trenéra
            v_raw := UTL_RAW.CAST_TO_RAW(
                        pocet || '. ' ||
                        t.jmeno || ' ' || t.prijmeni ||
                        ' – ' || t.pocetletpraxe || ' let praxe' || CHR(10)
                     );

            -- Zápis textu do BLOBu
            DBMS_LOB.WRITEAPPEND(v_blob, UTL_RAW.LENGTH(v_raw), v_raw);
        END LOOP;


        -- Pokud nebyl nalezen ani jeden trenér
        IF pocet = 0 THEN
            v_raw := UTL_RAW.CAST_TO_RAW('Žádní trenéøi nebyli nalezeni.' || CHR(10));
            DBMS_LOB.WRITEAPPEND(v_blob, UTL_RAW.LENGTH(v_raw), v_raw);
        END IF;
    END;

    -- Vracíme BLOB
    RETURN v_blob;

EXCEPTION

    WHEN OTHERS THEN
        RETURN NULL;
        
END F_TOP3_TRENERI_BLOB;

END PKG_TRENERI;

/
--------------------------------------------------------
--  DDL for Package Body PKG_TRENINKY
--------------------------------------------------------

  CREATE OR REPLACE EDITIONABLE PACKAGE BODY "ST72870"."PKG_TRENINKY" AS

    ------------------------------------------------------------------
    -- Pøidá nový trénink vedený trenérem
    -- Najde trenéra podle rodného èísla, vygeneruje nové ID tréninku 
    -- a uloží údaje o tréninku do tabulky TRENINKY
    ------------------------------------------------------------------
    PROCEDURE SP_ADD_TRENINK (
        v_prijmeni     IN CLENOVE_KLUBU.PRIJMENI%TYPE,
        v_rodne_cislo  IN CLENOVE_KLUBU.RODNE_CISLO%TYPE,
        v_datum        IN TRENINKY.DATUM%TYPE,
        v_misto        IN TRENINKY.MISTO%TYPE,
        v_popis        IN TRENINKY.POPIS%TYPE DEFAULT NULL
    ) AS
        v_id_clen_klubu CLENOVE_KLUBU.IDCLENKLUBU%TYPE;
        v_id_trenink    TRENINKY.IDTRENINK%TYPE;
    BEGIN
        -- Najde ID trenéra podle rodného èísla
        SELECT IDCLENKLUBU
        INTO v_id_clen_klubu
        FROM CLENOVE_KLUBU
        WHERE RODNE_CISLO = v_rodne_cislo
          AND TYPCLENA = 'Trener';

        -- Vygeneruje nové ID pro trénink
        SELECT SEKV_TRENINK.NEXTVAL INTO v_id_trenink FROM DUAL;

        -- Vloží nový záznam o tréninku
        INSERT INTO TRENINKY (IDTRENINK, DATUM, MISTO, POPIS, IDCLENKLUBU)
        VALUES (v_id_trenink, v_datum, v_misto, v_popis, v_id_clen_klubu);

        COMMIT;

    EXCEPTION
        WHEN NO_DATA_FOUND THEN
            ROLLBACK;
            RAISE_APPLICATION_ERROR(-20001, 'Trenér s daným rodným èíslem nebyl nalezen!');
        WHEN OTHERS THEN
            ROLLBACK;
            RAISE_APPLICATION_ERROR(-20010, 'Neoèekávaná chyba pøi pøidávání tréninku: ' || SQLERRM);
    END SP_ADD_TRENINK;


    ------------------------------------------------------------------
    -- Aktualizuje/Edituje existující trénink trenéra
    -- Najde trenéra podle rodného èísla a pøepíše údaje o tréninku
    ------------------------------------------------------------------
    PROCEDURE SP_UPDATE_TRENINK (
        v_prijmeni     IN CLENOVE_KLUBU.PRIJMENI%TYPE,
        v_rodne_cislo  IN CLENOVE_KLUBU.RODNE_CISLO%TYPE,
        v_datum        IN TRENINKY.DATUM%TYPE,
        v_misto        IN TRENINKY.MISTO%TYPE,
        v_popis        IN TRENINKY.POPIS%TYPE DEFAULT NULL
    ) AS
        v_id_clen_klubu CLENOVE_KLUBU.IDCLENKLUBU%TYPE;
    BEGIN
        -- Najde ID trenéra podle rodného èísla
        SELECT IDCLENKLUBU
        INTO v_id_clen_klubu
        FROM CLENOVE_KLUBU
        WHERE RODNE_CISLO = v_rodne_cislo
          AND TYPCLENA = 'Trener';

        -- Aktualizuje tréninkové údaje
        UPDATE TRENINKY
        SET DATUM = v_datum,
            MISTO = v_misto,
            POPIS = v_popis
        WHERE IDCLENKLUBU = v_id_clen_klubu;

        COMMIT;

    EXCEPTION
        WHEN NO_DATA_FOUND THEN
            ROLLBACK;
            RAISE_APPLICATION_ERROR(-20002, 'Trenér nebyl nalezen pro úpravu tréninku!');
        WHEN OTHERS THEN
            ROLLBACK;
            RAISE_APPLICATION_ERROR(-20011, 'Neoèekávaná chyba pøi úpravì tréninku: ' || SQLERRM);
    END SP_UPDATE_TRENINK;


    ------------------------------------------------------------------
    -- Odstraní všechny tréninky daného trenéra
    -- Vyhledá trenéra podle rodného èísla a smaže jeho tréninky
    ------------------------------------------------------------------
    PROCEDURE SP_DELETE_TRENINK (
        v_rodne_cislo IN CLENOVE_KLUBU.RODNE_CISLO%TYPE
    ) AS
        v_id_clen_klubu CLENOVE_KLUBU.IDCLENKLUBU%TYPE;
    BEGIN
        -- Najde trenéra podle rodného èísla
        SELECT IDCLENKLUBU
        INTO v_id_clen_klubu
        FROM CLENOVE_KLUBU
        WHERE RODNE_CISLO = v_rodne_cislo
          AND TYPCLENA = 'Trener';

        -- Smaže všechny jeho tréninky
        DELETE FROM TRENINKY WHERE IDCLENKLUBU = v_id_clen_klubu;

        COMMIT;

    EXCEPTION
        WHEN NO_DATA_FOUND THEN
            ROLLBACK;
            RAISE_APPLICATION_ERROR(-20003, 'Trenér nebyl nalezen pro smazání tréninku!');
        WHEN OTHERS THEN
            ROLLBACK;
            RAISE_APPLICATION_ERROR(-20012, 'Neoèekávaná chyba pøi mazání tréninku: ' || SQLERRM);
    END SP_DELETE_TRENINK;
    
    
------------------------------------------------------------------
-- Vypoèítá zatížení tréninkových dnù v daném mìsíci
------------------------------------------------------------------
PROCEDURE SP_VYPOCITEJ_ZATEZ_TRENINK_DNU (
    v_datum IN treninky.datum%type,
    v_info OUT VARCHAR2
) AS
    v_mesic NUMBER := EXTRACT(MONTH FROM v_datum);
    v_rok   NUMBER := EXTRACT(YEAR  FROM v_datum);

    v_po NUMBER := 0;
    v_ut NUMBER := 0;
    v_st NUMBER := 0;
    v_ct NUMBER := 0;
    v_pa NUMBER := 0;
    v_so NUMBER := 0;
    v_ne NUMBER := 0;

    v_total NUMBER := 0;
    v_max NUMBER := 0;
    v_min NUMBER := 0;

    v_nazevMax VARCHAR2(20);
    v_nazevMin VARCHAR2(20);

    v_report VARCHAR2(32767);
BEGIN

    -- Naètení dat z tréninkù pro daný mìsíc a rok
    SELECT 
        SUM(CASE WHEN TO_CHAR(datum,'D','NLS_DATE_LANGUAGE=CZECH')='1' THEN 1 END), -- 1 = Pondìlí
        SUM(CASE WHEN TO_CHAR(datum,'D','NLS_DATE_LANGUAGE=CZECH')='2' THEN 1 END), -- 2 = Úterý
        SUM(CASE WHEN TO_CHAR(datum,'D','NLS_DATE_LANGUAGE=CZECH')='3' THEN 1 END), -- 3 = Støeda
        SUM(CASE WHEN TO_CHAR(datum,'D','NLS_DATE_LANGUAGE=CZECH')='4' THEN 1 END), -- 4 = Ètvrtek
        SUM(CASE WHEN TO_CHAR(datum,'D','NLS_DATE_LANGUAGE=CZECH')='5' THEN 1 END), -- 5 = Pátek
        SUM(CASE WHEN TO_CHAR(datum,'D','NLS_DATE_LANGUAGE=CZECH')='6' THEN 1 END), -- 6 = Sobota
        SUM(CASE WHEN TO_CHAR(datum,'D','NLS_DATE_LANGUAGE=CZECH')='7' THEN 1 END)  -- 7 = Nedìle
    INTO v_po, v_ut, v_st, v_ct, v_pa, v_so, v_ne
    FROM TRENINKY
    WHERE EXTRACT(MONTH FROM datum) = v_mesic
      AND EXTRACT(YEAR  FROM datum) = v_rok;

    -- Výpoèet celkového poètu tréninkù
    v_total := NVL(v_po,0) + NVL(v_ut,0) + NVL(v_st,0) + NVL(v_ct,0) +
               NVL(v_pa,0) + NVL(v_so,0) + NVL(v_ne,0);

    IF v_total = 0 THEN
        v_info := 'V tomto období (' || TO_CHAR(v_datum,'MM.YYYY') || ') neprobìhl žádný trénink.';
        RETURN;
    END IF;

    -- Urèení maxima a minima
    v_max := GREATEST(NVL(v_po,0), NVL(v_ut,0), NVL(v_st,0), NVL(v_ct,0),
                      NVL(v_pa,0), NVL(v_so,0), NVL(v_ne,0));

    v_min := LEAST(NVL(v_po,0), NVL(v_ut,0), NVL(v_st,0), NVL(v_ct,0),
                   NVL(v_pa,0), NVL(v_so,0), NVL(v_ne,0));

    -- Pøiøazení dnù s max a min
    IF v_max = NVL(v_po,0) THEN
        v_nazevMax := 'Pondìlí';
    ELSIF v_max = NVL(v_ut,0) THEN
        v_nazevMax := 'Úterý';
    ELSIF v_max = NVL(v_st,0) THEN
        v_nazevMax := 'Støeda';
    ELSIF v_max = NVL(v_ct,0) THEN
        v_nazevMax := 'Ètvrtek';
    ELSIF v_max = NVL(v_pa,0) THEN
        v_nazevMax := 'Pátek';
    ELSIF v_max = NVL(v_so,0) THEN
        v_nazevMax := 'Sobota';
    ELSE
        v_nazevMax := 'Nedìle';
    END IF;

    IF v_min = NVL(v_po,0) THEN
        v_nazevMin := 'Pondìlí';
    ELSIF v_min = NVL(v_ut,0) THEN
        v_nazevMin := 'Úterý';
    ELSIF v_min = NVL(v_st,0) THEN
        v_nazevMin := 'Støeda';
    ELSIF v_min = NVL(v_ct,0) THEN
        v_nazevMin := 'Ètvrtek';
    ELSIF v_min = NVL(v_pa,0) THEN
        v_nazevMin := 'Pátek';
    ELSIF v_min = NVL(v_so,0) THEN
        v_nazevMin := 'Sobota';
    ELSE
        v_nazevMin := 'Nedìle';
    END IF;

    -- Sestavení reportu
    v_report :=
        'Report tréninkového zatížení pro ' || TO_CHAR(v_datum,'MM.YYYY') || CHR(10) ||
        '---------------------------------------------' || CHR(10) ||
        'Celkový poèet tréninkù: ' || v_total || CHR(10) ||
        'Pondìlí: '  || NVL(v_po,0) || ' (' || ROUND(NVL(v_po,0)/v_total*100,1) || '%)' || CHR(10) ||
        'Úterý: '    || NVL(v_ut,0) || ' (' || ROUND(NVL(v_ut,0)/v_total*100,1) || '%)' || CHR(10) ||
        'Støeda: '   || NVL(v_st,0) || ' (' || ROUND(NVL(v_st,0)/v_total*100,1) || '%)' || CHR(10) ||
        'Ètvrtek: '  || NVL(v_ct,0) || ' (' || ROUND(NVL(v_ct,0)/v_total*100,1) || '%)' || CHR(10) ||
        'Pátek: '    || NVL(v_pa,0) || ' (' || ROUND(NVL(v_pa,0)/v_total*100,1) || '%)' || CHR(10) ||
        'Sobota: '   || NVL(v_so,0) || ' (' || ROUND(NVL(v_so,0)/v_total*100,1) || '%)' || CHR(10) ||
        'Nedìle: '   || NVL(v_ne,0) || ' (' || ROUND(NVL(v_ne,0)/v_total*100,1) || '%)' || CHR(10) ||
        CHR(10) ||
        'Nejvíce tréninkù: ' || v_nazevMax || ' (' || v_max || ')' || CHR(10) ||
        'Nejménì tréninkù: ' || v_nazevMin || ' (' || v_min || ')';

    v_info := v_report;

END SP_VYPOCITEJ_ZATEZ_TRENINK_DNU;

END PKG_TRENINKY;

/
--------------------------------------------------------
--  DDL for Package Body PKG_USER_CONTEXT
--------------------------------------------------------

  CREATE OR REPLACE EDITIONABLE PACKAGE BODY "ST72870"."PKG_USER_CONTEXT" AS

    g_user VARCHAR2(100);

    PROCEDURE set_user(v_user VARCHAR2) IS
    BEGIN
        g_user := v_user;
    END;

    FUNCTION get_user RETURN VARCHAR2 IS
    BEGIN
        RETURN g_user;
    END;
    
END pkg_user_context;

/
--------------------------------------------------------
--  DDL for Package Body PKG_VYSLEDKY_ZAPASU
--------------------------------------------------------

  CREATE OR REPLACE EDITIONABLE PACKAGE BODY "ST72870"."PKG_VYSLEDKY_ZAPASU" AS

-- Procedura pro vytvoøení nového výsledku
PROCEDURE SP_ADD_VYSLEDEK_ZAPASU
(
    v_id_zapas IN vysledky_zapasu.idzapas%TYPE
,   v_vysledek IN vysledky_zapasu.vysledek%TYPE
,   v_pocet_zlutych_karet IN vysledky_zapasu.pocetzlutychkaret%TYPE
,   v_pocet_cervenych_karet IN vysledky_zapasu.pocetcervenychkaret%TYPE
,   v_pocet_goly_domaci IN vysledky_zapasu.pocetgolydomacitym%TYPE
,   v_pocet_goly_hoste IN vysledky_zapasu.pocetgolyhostetym%TYPE
)AS

    -- Definice vlastní výjimky pro nenalezení nadøazeného klíèe
    E_FOREIGN_KEY_NOT_FOUND EXCEPTION;

    -- Svázání vlastní výjimky s konkrétní chybou (ORA-02291)
    PRAGMA EXCEPTION_INIT(E_FOREIGN_KEY_NOT_FOUND, -2291);
    
BEGIN

    INSERT INTO vysledky_zapasu (idzapas, vysledek, pocetzlutychkaret, pocetcervenychkaret, pocetgolydomacitym, pocetgolyhostetym)
    VALUES (
        v_id_zapas, 
        v_vysledek, 
        v_pocet_zlutych_karet, 
        v_pocet_cervenych_karet, 
        v_pocet_goly_domaci, 
        v_pocet_goly_hoste
    );

    COMMIT;
    
    EXCEPTION   
        -- Pokud se poruší UNIQUE CONSTRAINT, tak se vyvolá vlastní výjimka -20002 
        WHEN DUP_VAL_ON_INDEX THEN
            RAISE_APPLICATION_ERROR(-20002, 'Chyba: Zápas s ID "' || v_id_zapas ||'" již v tabulce má výsledek!');
            
        -- Pokud se nenajde nadøazený klíè v jiné tabulce, tak se vyvolá vlastní výjimka -20003    
        WHEN E_FOREIGN_KEY_NOT_FOUND THEN
            RAISE_APPLICATION_ERROR(-20003, 'Chyba: Zápas s ID "' || v_id_zapas ||'" neexistuje (porušení FK constraint)!');
                        
        WHEN OTHERS THEN
            ROLLBACK;
            RAISE;

END SP_ADD_VYSLEDEK_ZAPASU;


-- Procedura pro odebrání urèitého výsledku
PROCEDURE SP_ODEBER_VYSLEDEK_ZAPASU
(
    v_id_zapas IN vysledky_zapasu.idzapas%TYPE
)AS
BEGIN

    DELETE FROM vysledky_zapasu
    WHERE idzapas = v_id_zapas;
    COMMIT;
    
    EXCEPTION       
        WHEN OTHERS THEN
            ROLLBACK;
            RAISE;
            
END SP_ODEBER_VYSLEDEK_ZAPASU;


-- Procedura pro editaci vybraného výsledku
PROCEDURE SP_UPDATE_VYSLEDEK_ZAPASU
(
    v_id_zapas IN vysledky_zapasu.idzapas%TYPE
,   v_vysledek IN vysledky_zapasu.vysledek%TYPE
,   v_pocet_zlutych_karet IN vysledky_zapasu.pocetzlutychkaret%TYPE
,   v_pocet_cervenych_karet IN vysledky_zapasu.pocetcervenychkaret%TYPE
,   v_pocet_goly_domaci IN vysledky_zapasu.pocetgolydomacitym%TYPE
,   v_pocet_goly_hoste IN vysledky_zapasu.pocetgolyhostetym%TYPE
)AS

BEGIN
    
    UPDATE vysledky_zapasu
    SET vysledek = v_vysledek,
        pocetzlutychkaret = v_pocet_zlutych_karet,
        pocetcervenychkaret = v_pocet_cervenych_karet,
        pocetgolydomacitym = v_pocet_goly_domaci,
        pocetgolyhostetym = v_pocet_goly_hoste
    WHERE idzapas = v_id_zapas;
    
     -- Pokud se nenajde žádný výsledek zápasu se zadaným ID, tak se vyvolá vlastní výjimka -20002 
    IF SQL%ROWCOUNT = 0 THEN
        RAISE_APPLICATION_ERROR(-20002, 'Chyba: Výsledek zápasu s ID "' || v_id_zapas || '" v tabulce neexistuje!');
    END IF;
    
    COMMIT;
    
    EXCEPTION

        WHEN OTHERS THEN
        ROLLBACK;
        RAISE;

END SP_UPDATE_VYSLEDEK_ZAPASU;


-- Funkce pro kontrolu validace formátu výsledku
FUNCTION FN_JE_FORMAT_VYSLEDKU_VALIDNI 
(
    v_vysledek IN VARCHAR2
    
) RETURN NUMBER 
AS
BEGIN

    -- Kontrola, zda není výsledek ve validním formátu (D:H) a nebo je delší jak 5 znakù
    -- Vrátí se 0 jako FALSE
    IF NOT REGEXP_LIKE(v_vysledek, '^[0-9]+:[0-9]+$') OR LENGTH(v_vysledek) > 5 THEN
        RETURN 0;
    END IF;
  
    -- Vrátí se 1 jako TRUE
    RETURN 1;
    
END FN_JE_FORMAT_VYSLEDKU_VALIDNI;

END PKG_VYSLEDKY_ZAPASU;

/
--------------------------------------------------------
--  DDL for Package Body PKG_ZAPASY
--------------------------------------------------------

  CREATE OR REPLACE EDITIONABLE PACKAGE BODY "ST72870"."PKG_ZAPASY" AS

-- Procedura pro vytvoøení nového zápasu
PROCEDURE SP_ADD_ZAPAS
(
    v_datum IN zapasy.datum%TYPE
,   v_id_soutez IN zapasy.idsoutez%TYPE
,   v_id_stav IN zapasy.stav_zapasu_idstav%TYPE
,   v_domaci_tym IN zapasy.domacitym%TYPE
,   v_hoste_tym IN zapasy.hostetym%TYPE
)AS    

BEGIN

    INSERT INTO zapasy (datum, idsoutez, stav_zapasu_idstav, domacitym, hostetym)
    VALUES (v_datum, v_id_soutez, v_id_stav, v_domaci_tym, v_hoste_tym);
    
    COMMIT;
    
    EXCEPTION
            
        WHEN OTHERS THEN
            ROLLBACK;
            RAISE;

END SP_ADD_ZAPAS;


-- Procedura pro odebrání urèitého zápasu
PROCEDURE SP_ODEBER_ZAPAS
(
    v_id_zapas IN zapasy.idzapas%TYPE
) AS 

BEGIN

    -- Odebrání výsledku zápasu, který chceme odebrat
    pkg_vysledky_zapasu.sp_odeber_vysledek_zapasu(v_id_zapas);

    DELETE FROM zapasy
    WHERE idzapas = v_id_zapas;
    
    -- Pokud se nenajde žádný zápas se zadaným ID, tak se vyvolá vlastní výjimka -20002 
    IF SQL%ROWCOUNT = 0 THEN
        RAISE_APPLICATION_ERROR(-20002, 'Chyba: Zápas s ID "' || v_id_zapas || '" v tabulce neexistuje!');
    END IF;
    
    COMMIT;
    
    EXCEPTION       
        WHEN OTHERS THEN
            ROLLBACK;
            RAISE;
            
END SP_ODEBER_ZAPAS;


-- Procedura pro editaci vybraného zápasu
PROCEDURE SP_UPDATE_ZAPAS
(
    v_id_zapas IN zapasy.idzapas%TYPE
,   v_datum IN zapasy.datum%TYPE
,   v_id_soutez IN zapasy.idsoutez%TYPE
,   v_id_stav IN zapasy.stav_zapasu_idstav%TYPE
,   v_domaci_tym IN zapasy.domacitym%TYPE
,   v_hoste_tym IN zapasy.hostetym%TYPE
)AS

BEGIN
    
    UPDATE zapasy
    SET datum = v_datum,
        idsoutez = v_id_soutez,
        stav_zapasu_idstav = v_id_stav,
        domacitym = v_domaci_tym,
        hostetym = v_hoste_tym
    WHERE idzapas = v_id_zapas;
    
     -- Pokud se nenajde žádný zápas se zadaným ID, tak se vyvolá vlastní výjimka -20002 
    IF SQL%ROWCOUNT = 0 THEN
        RAISE_APPLICATION_ERROR(-20002, 'Chyba: Zápas s ID "' || v_id_zapas || '" v tabulce neexistuje!');
    END IF;
    
    COMMIT;
    
    EXCEPTION

        WHEN OTHERS THEN
        ROLLBACK;
        RAISE;

END SP_UPDATE_ZAPAS;

----------------------------------------------------------------------
-- FUNKCE - F_STAV_ZAPASU_SHRNUTI
----------------------------------------------------------------------
FUNCTION F_STAV_ZAPASU_SHRNUTI
RETURN VARCHAR2
AS
    -- Poèty zápasù
    v_celkem        NUMBER := 0;
    v_odehrano      NUMBER := 0;
    v_budouci       NUMBER := 0;

    -- Procenta
    v_pct_odehrano  NUMBER := 0;
    v_pct_budouci   NUMBER := 0;

    -- Textové informace o zápasech
    v_posledni      VARCHAR2(200);
    v_nejblizsi     VARCHAR2(200);

    -- Výsledný text
    v_vysledek_report VARCHAR2(4000);
BEGIN
    --------------------------------------------------------------------
    -- 1) Získání poètu všech zápasù
    --------------------------------------------------------------------
    SELECT COUNT(*) INTO v_celkem FROM ZAPASY;

    IF v_celkem = 0 THEN
        RETURN 'V systému nejsou žádné zápasy.';
    END IF;

    --------------------------------------------------------------------
    -- 2) Poèet odehraných a budoucích zápasù
    --------------------------------------------------------------------
    SELECT 
        SUM(CASE WHEN UPPER(sz.STAVZAPASU) = 'ODEHRÁNO' THEN 1 ELSE 0 END),
        SUM(CASE WHEN UPPER(sz.STAVZAPASU) = 'BUDE SE HRÁT' THEN 1 ELSE 0 END)
    INTO v_odehrano, v_budouci
    FROM ZAPASY z
    JOIN STAV_ZAPASU sz ON z.STAV_ZAPASU_IDSTAV = sz.IDSTAV;

    --------------------------------------------------------------------
    -- 3) Výpoèet procentuálního zastoupení
    --------------------------------------------------------------------
    v_pct_odehrano := ROUND((v_odehrano / v_celkem) * 100, 1);
    v_pct_budouci  := ROUND((v_budouci  / v_celkem) * 100, 1);

    --------------------------------------------------------------------
    -- 4) Poslední odehraný zápas
    --------------------------------------------------------------------
    BEGIN
        SELECT 'Poslední odehraný: ' ||
               TO_CHAR(z.DATUM, 'DD.MM.YYYY HH24:MI') ||
               ' – ' || z.DOMACITYM || ' vs ' || z.HOSTETYM
        INTO v_posledni
        FROM ZAPASY z
        JOIN STAV_ZAPASU sz ON z.STAV_ZAPASU_IDSTAV = sz.IDSTAV
        WHERE UPPER(sz.STAVZAPASU) = 'ODEHRÁNO'
        ORDER BY z.DATUM DESC       -- seøaï je od nejnovìjšího
        FETCH FIRST 1 ROW ONLY;     -- vezmi jen jeden øádek = úplnì poslední odehraný zápas
    EXCEPTION
        WHEN NO_DATA_FOUND THEN
            v_posledni := 'Poslední odehraný zápas nebyl nalezen.';
    END;

    --------------------------------------------------------------------
    -- 5) Nejbližší budoucí zápas
    --------------------------------------------------------------------
    BEGIN
        SELECT 'Nejblíže se hraje: ' ||
               TO_CHAR(z.DATUM, 'DD.MM.YYYY HH24:MI') ||
               ' – ' || z.DOMACITYM || ' vs ' || z.HOSTETYM
        INTO v_nejblizsi
        FROM ZAPASY z
        JOIN STAV_ZAPASU sz ON z.STAV_ZAPASU_IDSTAV = sz.IDSTAV
        WHERE UPPER(sz.STAVZAPASU) = 'BUDE SE HRÁT'
          AND z.DATUM > SYSDATE     -- musí být v budoucím datu (po dnešním aktuálním datu) 
        ORDER BY z.DATUM ASC        -- seøadit od nejbližšího termínu
        FETCH FIRST 1 ROW ONLY;     -- vezmi jen první = nejbližší budoucí zápas
    EXCEPTION
        WHEN NO_DATA_FOUND THEN
            v_nejblizsi := 'Nejbližší budoucí zápas není naplánován';
    END;

    --------------------------------------------------------------------
    -- 6) Složení výsledného textu, CHR(10) = nový øádek
    --------------------------------------------------------------------
    v_vysledek_report :=
           'Celkem zápasù: ' || v_celkem || CHR(10) ||
           'Odehráno: ' || v_odehrano || ' (' || v_pct_odehrano || '%)' || CHR(10) ||
           'Bude se hrát: ' || v_budouci || ' (' || v_pct_budouci || '%)' || CHR(10) ||
           CHR(10) ||
           v_posledni || CHR(10) ||
           v_nejblizsi;

    RETURN v_vysledek_report;

EXCEPTION
    WHEN OTHERS THEN
        RETURN 'Chyba pøi vytváøení reportu: ' || SQLERRM;
END F_STAV_ZAPASU_SHRNUTI;

END PKG_ZAPASY;

/
--------------------------------------------------------
--  Constraints for Table CLENOVE_KLUBU
--------------------------------------------------------

  ALTER TABLE "ST72870"."CLENOVE_KLUBU" ADD CONSTRAINT "CHECK_RODNECISLO" CHECK (LENGTH(RODNE_CISLO) = 10) ENABLE;
  ALTER TABLE "ST72870"."CLENOVE_KLUBU" ADD CONSTRAINT "CHECK_TELEFONNICISLO" CHECK (REGEXP_LIKE(telefonnicislo, '^[0-9]{9,12}$')) ENABLE;
  ALTER TABLE "ST72870"."CLENOVE_KLUBU" ADD CONSTRAINT "CHECK_TYPCLENA" CHECK ( typclena = 'Hrac'
                                          OR typclena = 'Trener' ) ENABLE;
  ALTER TABLE "ST72870"."CLENOVE_KLUBU" ADD CONSTRAINT "CLENOVE_KLUBU_PK" PRIMARY KEY ("IDCLENKLUBU")
  USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "STUDENTI"  ENABLE;
  ALTER TABLE "ST72870"."CLENOVE_KLUBU" ADD CONSTRAINT "CLEN_KLUBU_RODNECISLO_UN" UNIQUE ("RODNE_CISLO")
  USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "STUDENTI"  ENABLE;
  ALTER TABLE "ST72870"."CLENOVE_KLUBU" MODIFY ("IDCLENKLUBU" NOT NULL ENABLE);
  ALTER TABLE "ST72870"."CLENOVE_KLUBU" MODIFY ("JMENO" NOT NULL ENABLE);
  ALTER TABLE "ST72870"."CLENOVE_KLUBU" MODIFY ("PRIJMENI" NOT NULL ENABLE);
  ALTER TABLE "ST72870"."CLENOVE_KLUBU" MODIFY ("TYPCLENA" NOT NULL ENABLE);
  ALTER TABLE "ST72870"."CLENOVE_KLUBU" MODIFY ("TELEFONNICISLO" NOT NULL ENABLE);
--------------------------------------------------------
--  Constraints for Table VYSLEDKY_ZAPASU
--------------------------------------------------------

  ALTER TABLE "ST72870"."VYSLEDKY_ZAPASU" ADD CONSTRAINT "CHECK_POCETCERVENYCHKARET" CHECK ( pocetcervenychkaret >= 0 ) ENABLE;
  ALTER TABLE "ST72870"."VYSLEDKY_ZAPASU" ADD CONSTRAINT "CHECK_POCETGOLYDOMACITYM" CHECK ( pocetgolydomacitym >= 0 ) ENABLE;
  ALTER TABLE "ST72870"."VYSLEDKY_ZAPASU" ADD CONSTRAINT "CHECK_POCETGOLYHOSTETYM" CHECK ( pocetgolyhostetym >= 0 ) ENABLE;
  ALTER TABLE "ST72870"."VYSLEDKY_ZAPASU" ADD CONSTRAINT "CHECK_POCETZLUTYCHKARET" CHECK ( pocetzlutychkaret >= 0 ) ENABLE;
  ALTER TABLE "ST72870"."VYSLEDKY_ZAPASU" MODIFY ("VYSLEDEK" NOT NULL ENABLE);
  ALTER TABLE "ST72870"."VYSLEDKY_ZAPASU" MODIFY ("POCETZLUTYCHKARET" NOT NULL ENABLE);
  ALTER TABLE "ST72870"."VYSLEDKY_ZAPASU" MODIFY ("POCETCERVENYCHKARET" NOT NULL ENABLE);
  ALTER TABLE "ST72870"."VYSLEDKY_ZAPASU" MODIFY ("IDZAPAS" NOT NULL ENABLE);
  ALTER TABLE "ST72870"."VYSLEDKY_ZAPASU" ADD CONSTRAINT "VYSLEDKY_ZAPASU_PK" PRIMARY KEY ("IDZAPAS")
  USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "STUDENTI"  ENABLE;
--------------------------------------------------------
--  Constraints for Table STAV_ZAPASU
--------------------------------------------------------

  ALTER TABLE "ST72870"."STAV_ZAPASU" ADD CONSTRAINT "STAV_ZAPASU_PK" PRIMARY KEY ("IDSTAV")
  USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "STUDENTI"  ENABLE;
  ALTER TABLE "ST72870"."STAV_ZAPASU" ADD CONSTRAINT "STAV_ZAPASU_STAVZAPASU_UN" UNIQUE ("STAVZAPASU")
  USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "STUDENTI"  ENABLE;
  ALTER TABLE "ST72870"."STAV_ZAPASU" MODIFY ("IDSTAV" NOT NULL ENABLE);
  ALTER TABLE "ST72870"."STAV_ZAPASU" MODIFY ("STAVZAPASU" NOT NULL ENABLE);
--------------------------------------------------------
--  Constraints for Table SOUTEZE
--------------------------------------------------------

  ALTER TABLE "ST72870"."SOUTEZE" ADD CONSTRAINT "SOUTEZE_PK" PRIMARY KEY ("IDSOUTEZ")
  USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "STUDENTI"  ENABLE;
  ALTER TABLE "ST72870"."SOUTEZE" MODIFY ("IDSOUTEZ" NOT NULL ENABLE);
  ALTER TABLE "ST72870"."SOUTEZE" MODIFY ("STARTDATUM" NOT NULL ENABLE);
  ALTER TABLE "ST72870"."SOUTEZE" MODIFY ("KONECDATUM" NOT NULL ENABLE);
  ALTER TABLE "ST72870"."SOUTEZE" MODIFY ("IDTYPSOUTEZE" NOT NULL ENABLE);
--------------------------------------------------------
--  Constraints for Table DISCIPLINARNI_OPATRENI
--------------------------------------------------------

  ALTER TABLE "ST72870"."DISCIPLINARNI_OPATRENI" ADD CONSTRAINT "CHECK_DELKATRESTU" CHECK ( delkatrestu > 0 ) ENABLE;
  ALTER TABLE "ST72870"."DISCIPLINARNI_OPATRENI" ADD CONSTRAINT "DISCIPLINARNI_OPATRENI_PK" PRIMARY KEY ("IDOPATRENI")
  USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "STUDENTI"  ENABLE;
  ALTER TABLE "ST72870"."DISCIPLINARNI_OPATRENI" MODIFY ("IDOPATRENI" NOT NULL ENABLE);
  ALTER TABLE "ST72870"."DISCIPLINARNI_OPATRENI" MODIFY ("DATUMOPATRENI" NOT NULL ENABLE);
  ALTER TABLE "ST72870"."DISCIPLINARNI_OPATRENI" MODIFY ("DELKATRESTU" NOT NULL ENABLE);
--------------------------------------------------------
--  Constraints for Table UZIVATELSKE_UCTY
--------------------------------------------------------

  ALTER TABLE "ST72870"."UZIVATELSKE_UCTY" MODIFY ("IDUZIVATELSKYUCET" NOT NULL ENABLE);
  ALTER TABLE "ST72870"."UZIVATELSKE_UCTY" MODIFY ("UZIVATELSKEJMENO" NOT NULL ENABLE);
  ALTER TABLE "ST72870"."UZIVATELSKE_UCTY" MODIFY ("HESLO" NOT NULL ENABLE);
  ALTER TABLE "ST72870"."UZIVATELSKE_UCTY" MODIFY ("SALT" NOT NULL ENABLE);
  ALTER TABLE "ST72870"."UZIVATELSKE_UCTY" MODIFY ("EMAIL" NOT NULL ENABLE);
  ALTER TABLE "ST72870"."UZIVATELSKE_UCTY" MODIFY ("IDROLE" NOT NULL ENABLE);
  ALTER TABLE "ST72870"."UZIVATELSKE_UCTY" MODIFY ("POSLEDNIPRIHLASENI" NOT NULL ENABLE);
  ALTER TABLE "ST72870"."UZIVATELSKE_UCTY" ADD CONSTRAINT "UNI_EMAIL" UNIQUE ("EMAIL")
  USING INDEX (CREATE UNIQUE INDEX "ST72870"."UZIVATEL_EMAIL_UN" ON "ST72870"."UZIVATELSKE_UCTY" ("EMAIL") 
  PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "STUDENTI" )  ENABLE;
  ALTER TABLE "ST72870"."UZIVATELSKE_UCTY" ADD CONSTRAINT "UNI_UCET_UZIVJMENO" UNIQUE ("UZIVATELSKEJMENO")
  USING INDEX (CREATE UNIQUE INDEX "ST72870"."UZIVATEL_JMENO_UN" ON "ST72870"."UZIVATELSKE_UCTY" ("UZIVATELSKEJMENO") 
  PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "STUDENTI" )  ENABLE;
  ALTER TABLE "ST72870"."UZIVATELSKE_UCTY" ADD CONSTRAINT "UZIVATELSKY_UCET_PK" PRIMARY KEY ("IDUZIVATELSKYUCET")
  USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "STUDENTI"  ENABLE;
--------------------------------------------------------
--  Constraints for Table LOG_TABLE
--------------------------------------------------------

  ALTER TABLE "ST72870"."LOG_TABLE" ADD CONSTRAINT "LOG_TABLE_PK" PRIMARY KEY ("IDLOG")
  USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "STUDENTI"  ENABLE;
  ALTER TABLE "ST72870"."LOG_TABLE" MODIFY ("IDLOG" NOT NULL ENABLE);
  ALTER TABLE "ST72870"."LOG_TABLE" MODIFY ("OPERACE" NOT NULL ENABLE);
  ALTER TABLE "ST72870"."LOG_TABLE" MODIFY ("CAS" NOT NULL ENABLE);
  ALTER TABLE "ST72870"."LOG_TABLE" MODIFY ("UZIVATEL" NOT NULL ENABLE);
  ALTER TABLE "ST72870"."LOG_TABLE" MODIFY ("TABULKA" NOT NULL ENABLE);
--------------------------------------------------------
--  Constraints for Table ROLE
--------------------------------------------------------

  ALTER TABLE "ST72870"."ROLE" MODIFY ("NAZEVROLE" NOT NULL ENABLE);
  ALTER TABLE "ST72870"."ROLE" ADD PRIMARY KEY ("IDROLE")
  USING INDEX (CREATE UNIQUE INDEX "ST72870"."ROLE_PK" ON "ST72870"."ROLE" ("IDROLE") 
  PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "STUDENTI" )  ENABLE;
  ALTER TABLE "ST72870"."ROLE" ADD UNIQUE ("NAZEVROLE")
  USING INDEX (CREATE UNIQUE INDEX "ST72870"."ROLE_NAZEVROLE_UN" ON "ST72870"."ROLE" ("NAZEVROLE") 
  PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "STUDENTI" )  ENABLE;
--------------------------------------------------------
--  Constraints for Table SPONZORI
--------------------------------------------------------

  ALTER TABLE "ST72870"."SPONZORI" ADD CONSTRAINT "CHECK_SPONZOROVANACASTKA" CHECK ( sponzorovanacastka > 0 ) ENABLE;
  ALTER TABLE "ST72870"."SPONZORI" ADD CONSTRAINT "SPONZORI_JMENO_UN" UNIQUE ("JMENO")
  USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "STUDENTI"  ENABLE;
  ALTER TABLE "ST72870"."SPONZORI" ADD CONSTRAINT "SPONZORI_PK" PRIMARY KEY ("IDSPONZOR")
  USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "STUDENTI"  ENABLE;
  ALTER TABLE "ST72870"."SPONZORI" MODIFY ("IDSPONZOR" NOT NULL ENABLE);
  ALTER TABLE "ST72870"."SPONZORI" MODIFY ("JMENO" NOT NULL ENABLE);
--------------------------------------------------------
--  Constraints for Table SPONZORI_CLENOVE
--------------------------------------------------------

  ALTER TABLE "ST72870"."SPONZORI_CLENOVE" ADD CONSTRAINT "SPONZORI_CLENOVE_PK" PRIMARY KEY ("IDSPONZOR", "IDCLENKLUBU")
  USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "STUDENTI"  ENABLE;
  ALTER TABLE "ST72870"."SPONZORI_CLENOVE" MODIFY ("IDSPONZOR" NOT NULL ENABLE);
  ALTER TABLE "ST72870"."SPONZORI_CLENOVE" MODIFY ("IDCLENKLUBU" NOT NULL ENABLE);
--------------------------------------------------------
--  Constraints for Table TRENERI
--------------------------------------------------------

  ALTER TABLE "ST72870"."TRENERI" ADD CONSTRAINT "CHECK_POCETLETPRAXE" CHECK ( pocetletpraxe > 0 ) ENABLE;
  ALTER TABLE "ST72870"."TRENERI" MODIFY ("IDCLENKLUBU" NOT NULL ENABLE);
  ALTER TABLE "ST72870"."TRENERI" MODIFY ("TRENERSKALICENCE" NOT NULL ENABLE);
  ALTER TABLE "ST72870"."TRENERI" MODIFY ("POCETLETPRAXE" NOT NULL ENABLE);
  ALTER TABLE "ST72870"."TRENERI" ADD CONSTRAINT "TRENERI_PK" PRIMARY KEY ("IDCLENKLUBU")
  USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "STUDENTI"  ENABLE;
--------------------------------------------------------
--  Constraints for Table POZICE_HRAC
--------------------------------------------------------

  ALTER TABLE "ST72870"."POZICE_HRAC" MODIFY ("NAZEV_POZICE" NOT NULL ENABLE);
  ALTER TABLE "ST72870"."POZICE_HRAC" ADD PRIMARY KEY ("ID_POZICE")
  USING INDEX (CREATE UNIQUE INDEX "ST72870"."POZICE_HRAC_PK" ON "ST72870"."POZICE_HRAC" ("ID_POZICE") 
  PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "STUDENTI" )  ENABLE;
  ALTER TABLE "ST72870"."POZICE_HRAC" ADD UNIQUE ("NAZEV_POZICE")
  USING INDEX (CREATE UNIQUE INDEX "ST72870"."POZICE_HRAC_NAZEVPOZICE_UN" ON "ST72870"."POZICE_HRAC" ("NAZEV_POZICE") 
  PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "STUDENTI" )  ENABLE;
--------------------------------------------------------
--  Constraints for Table SPONZORI_SOUTEZE
--------------------------------------------------------

  ALTER TABLE "ST72870"."SPONZORI_SOUTEZE" ADD CONSTRAINT "SPONZORI_SOUTEZE_PK" PRIMARY KEY ("IDSPONZOR", "IDSOUTEZ")
  USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "STUDENTI"  ENABLE;
  ALTER TABLE "ST72870"."SPONZORI_SOUTEZE" MODIFY ("IDSPONZOR" NOT NULL ENABLE);
  ALTER TABLE "ST72870"."SPONZORI_SOUTEZE" MODIFY ("IDSOUTEZ" NOT NULL ENABLE);
--------------------------------------------------------
--  Constraints for Table TYP_SOUTEZ
--------------------------------------------------------

  ALTER TABLE "ST72870"."TYP_SOUTEZ" MODIFY ("IDTYPSOUTEZE" NOT NULL ENABLE);
  ALTER TABLE "ST72870"."TYP_SOUTEZ" MODIFY ("NAZEVSOUTEZE" NOT NULL ENABLE);
  ALTER TABLE "ST72870"."TYP_SOUTEZ" ADD CONSTRAINT "TYP_SOUTEZ_NAZEVSOUTEZE_UN" UNIQUE ("NAZEVSOUTEZE")
  USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "STUDENTI"  ENABLE;
  ALTER TABLE "ST72870"."TYP_SOUTEZ" ADD CONSTRAINT "TYP_SOUTEZ_PK" PRIMARY KEY ("IDTYPSOUTEZE")
  USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "STUDENTI"  ENABLE;
--------------------------------------------------------
--  Constraints for Table HRACI
--------------------------------------------------------

  ALTER TABLE "ST72870"."HRACI" ADD CONSTRAINT "CHECK_POCETCERVENYCHKARET_HRACI" CHECK (pocet_Cervenych_Karet >= 0) ENABLE;
  ALTER TABLE "ST72870"."HRACI" ADD CONSTRAINT "CHECK_POCETVSTRELENYCHGOLU" CHECK ( pocetvstrelenychgolu >= 0 ) ENABLE;
  ALTER TABLE "ST72870"."HRACI" ADD CONSTRAINT "CHECK_POCETZLUTYCHKARET_HRACI" CHECK (pocet_Zlutych_Karet >= 0) ENABLE;
  ALTER TABLE "ST72870"."HRACI" ADD CONSTRAINT "HRACI_PK" PRIMARY KEY ("IDCLENKLUBU")
  USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "STUDENTI"  ENABLE;
  ALTER TABLE "ST72870"."HRACI" MODIFY ("IDCLENKLUBU" NOT NULL ENABLE);
  ALTER TABLE "ST72870"."HRACI" MODIFY ("POCETVSTRELENYCHGOLU" NOT NULL ENABLE);
  ALTER TABLE "ST72870"."HRACI" MODIFY ("ID_POZICE" NOT NULL ENABLE);
  ALTER TABLE "ST72870"."HRACI" MODIFY ("POCET_ZLUTYCH_KARET" NOT NULL ENABLE);
  ALTER TABLE "ST72870"."HRACI" MODIFY ("POCET_CERVENYCH_KARET" NOT NULL ENABLE);
--------------------------------------------------------
--  Constraints for Table ZAPASY
--------------------------------------------------------

  ALTER TABLE "ST72870"."ZAPASY" MODIFY ("IDZAPAS" NOT NULL ENABLE);
  ALTER TABLE "ST72870"."ZAPASY" MODIFY ("DATUM" NOT NULL ENABLE);
  ALTER TABLE "ST72870"."ZAPASY" MODIFY ("IDSOUTEZ" NOT NULL ENABLE);
  ALTER TABLE "ST72870"."ZAPASY" MODIFY ("STAV_ZAPASU_IDSTAV" NOT NULL ENABLE);
  ALTER TABLE "ST72870"."ZAPASY" MODIFY ("DOMACITYM" NOT NULL ENABLE);
  ALTER TABLE "ST72870"."ZAPASY" MODIFY ("HOSTETYM" NOT NULL ENABLE);
  ALTER TABLE "ST72870"."ZAPASY" ADD CONSTRAINT "ZAPASY_PK" PRIMARY KEY ("IDZAPAS")
  USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "STUDENTI"  ENABLE;
--------------------------------------------------------
--  Constraints for Table BINARNI_OBSAH
--------------------------------------------------------

  ALTER TABLE "ST72870"."BINARNI_OBSAH" ADD CONSTRAINT "BINARNI_OBSAH_PK" PRIMARY KEY ("IDBINARNIOBSAH")
  USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "STUDENTI"  ENABLE;
  ALTER TABLE "ST72870"."BINARNI_OBSAH" MODIFY ("IDBINARNIOBSAH" NOT NULL ENABLE);
  ALTER TABLE "ST72870"."BINARNI_OBSAH" MODIFY ("NAZEVSOUBORU" NOT NULL ENABLE);
  ALTER TABLE "ST72870"."BINARNI_OBSAH" MODIFY ("TYPSOUBORU" NOT NULL ENABLE);
  ALTER TABLE "ST72870"."BINARNI_OBSAH" MODIFY ("PRIPONASOUBORU" NOT NULL ENABLE);
  ALTER TABLE "ST72870"."BINARNI_OBSAH" MODIFY ("OBSAH" NOT NULL ENABLE);
  ALTER TABLE "ST72870"."BINARNI_OBSAH" MODIFY ("DATUMNAHRANI" NOT NULL ENABLE);
  ALTER TABLE "ST72870"."BINARNI_OBSAH" MODIFY ("DATUMMODIFIKACE" NOT NULL ENABLE);
  ALTER TABLE "ST72870"."BINARNI_OBSAH" MODIFY ("OPERACE" NOT NULL ENABLE);
  ALTER TABLE "ST72870"."BINARNI_OBSAH" MODIFY ("IDUZIVATELSKYUCET" NOT NULL ENABLE);
--------------------------------------------------------
--  Constraints for Table TRENINKY
--------------------------------------------------------

  ALTER TABLE "ST72870"."TRENINKY" MODIFY ("IDTRENINK" NOT NULL ENABLE);
  ALTER TABLE "ST72870"."TRENINKY" MODIFY ("DATUM" NOT NULL ENABLE);
  ALTER TABLE "ST72870"."TRENINKY" MODIFY ("MISTO" NOT NULL ENABLE);
  ALTER TABLE "ST72870"."TRENINKY" MODIFY ("IDCLENKLUBU" NOT NULL ENABLE);
  ALTER TABLE "ST72870"."TRENINKY" ADD CONSTRAINT "TRENINKY_PK" PRIMARY KEY ("IDTRENINK")
  USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "STUDENTI"  ENABLE;
--------------------------------------------------------
--  Constraints for Table KONTRAKTY
--------------------------------------------------------

  ALTER TABLE "ST72870"."KONTRAKTY" ADD CONSTRAINT "CHECK_CISLONAAGENTA" CHECK (cislonaagenta BETWEEN 100000000 AND 999999999999) ENABLE;
  ALTER TABLE "ST72870"."KONTRAKTY" ADD CONSTRAINT "CHECK_PLAT" CHECK (plat >= 	20800) ENABLE;
  ALTER TABLE "ST72870"."KONTRAKTY" ADD CONSTRAINT "CHECK_VYSTUPNIKLAUZULE" CHECK ( vystupniklauzule > 0 ) ENABLE;
  ALTER TABLE "ST72870"."KONTRAKTY" ADD CONSTRAINT "KONTRAKTY_PK" PRIMARY KEY ("IDCLENKLUBU")
  USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "STUDENTI"  ENABLE;
  ALTER TABLE "ST72870"."KONTRAKTY" MODIFY ("DATUMZACATKU" NOT NULL ENABLE);
  ALTER TABLE "ST72870"."KONTRAKTY" MODIFY ("DATUMKONCE" NOT NULL ENABLE);
  ALTER TABLE "ST72870"."KONTRAKTY" MODIFY ("PLAT" NOT NULL ENABLE);
  ALTER TABLE "ST72870"."KONTRAKTY" MODIFY ("CISLONAAGENTA" NOT NULL ENABLE);
  ALTER TABLE "ST72870"."KONTRAKTY" MODIFY ("IDCLENKLUBU" NOT NULL ENABLE);
--------------------------------------------------------
--  Ref Constraints for Table BINARNI_OBSAH
--------------------------------------------------------

  ALTER TABLE "ST72870"."BINARNI_OBSAH" ADD CONSTRAINT "BIN_OBSAHUZIVATELSKE_UCTY_FK" FOREIGN KEY ("IDUZIVATELSKYUCET")
	  REFERENCES "ST72870"."UZIVATELSKE_UCTY" ("IDUZIVATELSKYUCET") ENABLE;
--------------------------------------------------------
--  Ref Constraints for Table HRACI
--------------------------------------------------------

  ALTER TABLE "ST72870"."HRACI" ADD CONSTRAINT "HRACI_CLENOVE_KLUBU_FK" FOREIGN KEY ("IDCLENKLUBU")
	  REFERENCES "ST72870"."CLENOVE_KLUBU" ("IDCLENKLUBU") ENABLE;
  ALTER TABLE "ST72870"."HRACI" ADD CONSTRAINT "HRACI_OPATRENI_FK" FOREIGN KEY ("IDOPATRENI")
	  REFERENCES "ST72870"."DISCIPLINARNI_OPATRENI" ("IDOPATRENI") ENABLE;
--------------------------------------------------------
--  Ref Constraints for Table KONTRAKTY
--------------------------------------------------------

  ALTER TABLE "ST72870"."KONTRAKTY" ADD CONSTRAINT "KONTRAKTY_HRACI_FK" FOREIGN KEY ("IDCLENKLUBU")
	  REFERENCES "ST72870"."HRACI" ("IDCLENKLUBU") ON DELETE CASCADE ENABLE;
--------------------------------------------------------
--  Ref Constraints for Table SOUTEZE
--------------------------------------------------------

  ALTER TABLE "ST72870"."SOUTEZE" ADD CONSTRAINT "SOUTEZ_TYP_SOUTEZ_FK" FOREIGN KEY ("IDTYPSOUTEZE")
	  REFERENCES "ST72870"."TYP_SOUTEZ" ("IDTYPSOUTEZE") ENABLE;
--------------------------------------------------------
--  Ref Constraints for Table SPONZORI_CLENOVE
--------------------------------------------------------

  ALTER TABLE "ST72870"."SPONZORI_CLENOVE" ADD CONSTRAINT "SPONZOR_CLEN_CLEN_KLUBU_FK" FOREIGN KEY ("IDCLENKLUBU")
	  REFERENCES "ST72870"."CLENOVE_KLUBU" ("IDCLENKLUBU") ENABLE;
  ALTER TABLE "ST72870"."SPONZORI_CLENOVE" ADD CONSTRAINT "SPONZOR_CLEN_SPONZOR_FK" FOREIGN KEY ("IDSPONZOR")
	  REFERENCES "ST72870"."SPONZORI" ("IDSPONZOR") ENABLE;
--------------------------------------------------------
--  Ref Constraints for Table SPONZORI_SOUTEZE
--------------------------------------------------------

  ALTER TABLE "ST72870"."SPONZORI_SOUTEZE" ADD CONSTRAINT "SPONZORI_SOUTEZE_SOUTEZE_FK" FOREIGN KEY ("IDSOUTEZ")
	  REFERENCES "ST72870"."SOUTEZE" ("IDSOUTEZ") ENABLE;
  ALTER TABLE "ST72870"."SPONZORI_SOUTEZE" ADD CONSTRAINT "SPONZORI_SOUTEZE_SPONZORI_FK" FOREIGN KEY ("IDSPONZOR")
	  REFERENCES "ST72870"."SPONZORI" ("IDSPONZOR") ENABLE;
--------------------------------------------------------
--  Ref Constraints for Table TRENERI
--------------------------------------------------------

  ALTER TABLE "ST72870"."TRENERI" ADD CONSTRAINT "TRENERI_CLENOVE_KLUBU_FK" FOREIGN KEY ("IDCLENKLUBU")
	  REFERENCES "ST72870"."CLENOVE_KLUBU" ("IDCLENKLUBU") ENABLE;
--------------------------------------------------------
--  Ref Constraints for Table TRENINKY
--------------------------------------------------------

  ALTER TABLE "ST72870"."TRENINKY" ADD CONSTRAINT "TRENINKY_TRENERI_FK" FOREIGN KEY ("IDCLENKLUBU")
	  REFERENCES "ST72870"."TRENERI" ("IDCLENKLUBU") ENABLE;
--------------------------------------------------------
--  Ref Constraints for Table UZIVATELSKE_UCTY
--------------------------------------------------------

  ALTER TABLE "ST72870"."UZIVATELSKE_UCTY" ADD CONSTRAINT "FK_UZIVATEL_CLEN" FOREIGN KEY ("CLEN_KLUBU_IDCLENKLUBU")
	  REFERENCES "ST72870"."CLENOVE_KLUBU" ("IDCLENKLUBU") ENABLE;
--------------------------------------------------------
--  Ref Constraints for Table VYSLEDKY_ZAPASU
--------------------------------------------------------

  ALTER TABLE "ST72870"."VYSLEDKY_ZAPASU" ADD CONSTRAINT "VYSLEDKY_ZAPASU_ZAPASY_FK" FOREIGN KEY ("IDZAPAS")
	  REFERENCES "ST72870"."ZAPASY" ("IDZAPAS") ENABLE;
--------------------------------------------------------
--  Ref Constraints for Table ZAPASY
--------------------------------------------------------

  ALTER TABLE "ST72870"."ZAPASY" ADD CONSTRAINT "ZAPAS_SOUTEZ_FK" FOREIGN KEY ("IDSOUTEZ")
	  REFERENCES "ST72870"."SOUTEZE" ("IDSOUTEZ") ENABLE;
  ALTER TABLE "ST72870"."ZAPASY" ADD CONSTRAINT "ZAPAS_STAV_ZAPASU_FK" FOREIGN KEY ("STAV_ZAPASU_IDSTAV")
	  REFERENCES "ST72870"."STAV_ZAPASU" ("IDSTAV") ENABLE;
