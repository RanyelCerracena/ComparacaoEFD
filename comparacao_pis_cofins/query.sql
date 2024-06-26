CREATE TEMPORARY FUNCTION MMS_D_BUSCA_VIGENCIA_IMPOSTO(IN AIEMPRESA INTEGER, IN AIIMPOSTO INTEGER, IN ADVIGENCIA DATE)
RETURNS DATE
BEGIN
	DECLARE LDRETORNO DATE;

	SET LDRETORNO = (SELECT MAX(Q.VIGENCIA_IMP) 
							 FROM BETHADBA.GEIMPOSTO_VIGENCIA AS Q 
							WHERE Q.CODI_EMP = AIEMPRESA AND 
									Q.CODI_IMP = AIIMPOSTO AND 
									Q.VIGENCIA_IMP <= ADVIGENCIA);
	
	RETURN LDRETORNO;
END;
//Remover ou comentar dessa linha para cima após o primeiro uso

CREATE OR REPLACE VARIABLE dataApuracao DATE = '#data';

SELECT SI.sdev_sim AS 'Valor a recolher'       
			 FROM BETHADBA.GEEMPRE AS G INNER JOIN 
					BETHADBA.EFSDOIMP AS SI 
				ON SI.CODI_EMP = G.CODI_EMP INNER JOIN 
					BETHADBA.GEIMPOSTO AS I 
				ON I.CODI_EMP = SI.CODI_EMP AND 
					I.CODI_IMP = SI.CODI_IMP INNER JOIN 
					BETHADBA.GEIMPOSTO_VIGENCIA AS VI 
				ON VI.CODI_EMP = SI.CODI_EMP AND 
					VI.CODI_IMP = SI.CODI_IMP,
					LATERAL(SELECT COALESCE(SUM(EFSDOIMP.VDI64_SIM), 0) AS OUTRAS_COMPENSACOES,
										COALESCE(SUM(EFSDOIMP.VDI65_SIM), 0) AS PAGAMENTO_INDEVIDO_OU_MAIOR
								 FROM BETHADBA.EFSDOIMP AS EFSDOIMP
								WHERE EFSDOIMP.CODI_EMP = SI.CODI_EMP AND 
									   EFSDOIMP.CODI_IMP = 6 AND 
									   EFSDOIMP.DATA_SIM = SI.DATA_SIM AND 
									   EFSDOIMP.PDIC_SIM = SI.PDIC_SIM AND
										EFSDOIMP.CODI_IMP = SI.CODI_IMP) AS TD_COMPENSACAO_CSOC,
					LATERAL(SELECT COALESCE(SUM(EFSDOIMP.VDI65_SIM), 0) AS OUTRAS_COMPENSACOES,
										COALESCE(SUM(EFSDOIMP.VDI66_SIM), 0) AS PAGAMENTO_INDEVIDO_OU_MAIOR
								 FROM BETHADBA.EFSDOIMP AS EFSDOIMP
								WHERE EFSDOIMP.CODI_EMP = SI.CODI_EMP AND 
									   EFSDOIMP.CODI_IMP = 7 AND 
									   EFSDOIMP.DATA_SIM = SI.DATA_SIM AND 
									   EFSDOIMP.PDIC_SIM = SI.PDIC_SIM AND
										EFSDOIMP.CODI_IMP = SI.CODI_IMP) AS TD_COMPENSACAO_IRPJ,
					LATERAL(SELECT COALESCE(SUM(EFSDOIMP.VDI1_SIM), 0) AS OUTRAS_COMPENSACOES,
										COALESCE(SUM(EFSDOIMP.VDI2_SIM), 0) AS PAGAMENTO_INDEVIDO_OU_MAIOR
								 FROM BETHADBA.EFSDOIMP AS EFSDOIMP
								WHERE EFSDOIMP.CODI_EMP = SI.CODI_EMP AND 
									   EFSDOIMP.CODI_IMP = 41 AND 
									   EFSDOIMP.DATA_SIM = SI.DATA_SIM AND 
									   EFSDOIMP.PDIC_SIM = SI.PDIC_SIM AND
										EFSDOIMP.CODI_IMP = SI.CODI_IMP) AS TD_COMPENSACAO_FUNTTEL,
					LATERAL(SELECT COALESCE(SUM(EFSDOIMP.VDI27_SIM), 0) AS OUTRAS_COMPENSACOES,
										COALESCE(SUM(EFSDOIMP.VDI70_SIM ), 0) AS PAGAMENTO_INDEVIDO_OU_MAIOR
								 FROM BETHADBA.EFSDOIMP AS EFSDOIMP
								WHERE EFSDOIMP.CODI_EMP = SI.CODI_EMP AND 
									   EFSDOIMP.CODI_IMP IN (4, 5, 17, 19) AND 
									   EFSDOIMP.DATA_SIM = SI.DATA_SIM AND 
									   EFSDOIMP.PDIC_SIM = SI.PDIC_SIM AND
										EFSDOIMP.CODI_IMP = SI.CODI_IMP) AS TD_COMPENSACAO_PIS_COFINS,
					LATERAL(SELECT COALESCE(SUM(EFSDOIMP.VDI5_SIM), 0) AS OUTRAS_COMPENSACOES,
										COALESCE(SUM(EFSDOIMP.VDI6_SIM ), 0) AS PAGAMENTO_INDEVIDO_OU_MAIOR
								 FROM BETHADBA.EFSDOIMP AS EFSDOIMP
								WHERE EFSDOIMP.CODI_EMP = SI.CODI_EMP AND 
									   EFSDOIMP.CODI_IMP = 30 AND 
									   EFSDOIMP.DATA_SIM = SI.DATA_SIM AND 
									   EFSDOIMP.PDIC_SIM = SI.PDIC_SIM AND
										EFSDOIMP.CODI_IMP = SI.CODI_IMP) AS TD_COMPENSACAO_IPIM,
					LATERAL(SELECT TD_COMPENSACAO_CSOC.OUTRAS_COMPENSACOES + TD_COMPENSACAO_CSOC.PAGAMENTO_INDEVIDO_OU_MAIOR +
										TD_COMPENSACAO_IRPJ.OUTRAS_COMPENSACOES + TD_COMPENSACAO_IRPJ.PAGAMENTO_INDEVIDO_OU_MAIOR +
										TD_COMPENSACAO_FUNTTEL.OUTRAS_COMPENSACOES + TD_COMPENSACAO_FUNTTEL.PAGAMENTO_INDEVIDO_OU_MAIOR + 
										TD_COMPENSACAO_PIS_COFINS.OUTRAS_COMPENSACOES + TD_COMPENSACAO_PIS_COFINS.PAGAMENTO_INDEVIDO_OU_MAIOR +
										TD_COMPENSACAO_IPIM.OUTRAS_COMPENSACOES + TD_COMPENSACAO_IPIM.PAGAMENTO_INDEVIDO_OU_MAIOR AS VALOR_COMPENSACOES
								 FROM DSDBA.DUMMY) AS TD_COMPENSACAO_APURACAO,
					LATERAL(SELECT (CASE WHEN SI.CODI_IMP IN (4,5,6,7,10,17,19,39,41,66) THEN 
											SI.SDIF_SIM 
										ELSE 
											0 
										END) AS DIFERIDO 
								 FROM DSDBA.DUMMY) AS TDAUX
			WHERE VI.VIGENCIA_IMP = MMS_D_BUSCA_VIGENCIA_IMPOSTO(SI.CODI_EMP, SI.CODI_IMP, dataApuracao) AND
					SI.CODI_IMP IN (#codImposto) AND 
					SI.CODI_EMP IN (#codEmpresa) AND
					SI.DATA_SIM = dataApuracao ORDER BY SI.CODI_EMP