using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DNA
{
	private int iDnaLength, fDnaLength, iMinVal, iMaxVal;
	private float fMinVal, fMaxVal;
	[SerializeField]
	private List<int> iGenes = new List<int>(); // 0 = nothing // 1 = jump // 2 = crouch
	[SerializeField]
	private List<float> fGenes = new List<float>(); // forward, right [-1, 1]

	public DNA(int il, int iMin, int iMax, int fl, float fMin, float fMax)
	{
		iDnaLength = il;
		fDnaLength = fl;
		fMinVal = fMin;
		fMaxVal = fMax;
		iMinVal = iMin;
		iMaxVal = iMax;
		SetRandom();
	}

	public void SetRandom()
	{
		iGenes.Clear();
		for (int i = 0; i < iDnaLength; i++)
			iGenes.Add(Random.Range(iMinVal, iMaxVal));
		fGenes.Clear();
		for (int i = 0; i < fDnaLength; i++)
			fGenes.Add(Random.Range(fMinVal, fMaxVal));
	}

	public void SetInt(int pos, int value)
	{
		iGenes[pos] = value;
	}
	public void SetFloat(int pos, float value)
	{
		fGenes[pos] = value;
	}

	public void Combine(DNA d1, DNA d2, float mutationChance)
	{
		for(int i = 0; i < iDnaLength; i++)
		{
			if (Random.Range(0.0f, 1.0f) < mutationChance)
				iGenes[i] = Random.Range(iMinVal, iMaxVal);
			else
				iGenes[i] = Random.Range(0, 10) < 5 ? d1.GetIGene(i) : d2.GetIGene(i);
		}
		for (int i = 0; i < fDnaLength; i++)
		{
			if (Random.Range(0.0f, 1.0f) < mutationChance)
				fGenes[i] = Random.Range(fMinVal, fMaxVal);
			else
				fGenes[i] = Random.Range(0, 10) < 5 ? d1.GetFGene(i) : d2.GetFGene(i);
		}
	}

	public int GetIGene(int pos)
	{
		return iGenes[pos];
	}
	public float GetFGene(int pos)
	{
		return fGenes[pos];
	}

}
