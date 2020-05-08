using System;
using System.Linq;
using System.Text;


namespace NSGA_II
{
    public class Genotype     
    {
        static Random random = new Random();

        public string gene;
        public double[] genes;


        public Genotype(int nBytes = 16)
        {
            for (int i = 0; i < nBytes; i++)
                gene += random.NextDouble() < 0.5 ? "1" : "0";

            genes = new double[2];
            for (int i = 0; i < genes.Length; i++)
            {
                genes[i] = random.NextDouble();
            }
        }


        // Mutate: Gives genes a small probability (1/16) of mutation
        public void Mutate()
        {
            StringBuilder newGene = new StringBuilder(gene, gene.Length);
            double probabilityMutation = 1.0 / gene.Length;

            for (int i = 0; i < gene.Length; i++)
                if (random.NextDouble() < probabilityMutation)
                    newGene[i] = (newGene[i] == '1') ? '0' : '1';

            for (int i = 0; i < genes.Length; i++)
                if (random.NextDouble() < probabilityMutation)
                    genes[i] = random.NextDouble();

            gene = newGene.ToString();
        }


        // Decode: Remaps the gene's binary code (string) to a number between 0 and 1
        public double Decode()
        {
            int sum = 0;
            for (int i = gene.Length - 1; i >= 0; i--)
                sum += ((gene[i] == '1') ? 1 : 0) * (int)Math.Pow(2.0, gene.Length - 1 - i);

            double max = Enumerable.Range(0, gene.Length).Sum(i => Math.Pow(2.0, i));
            double decodedGene = sum / max;
            return decodedGene;
        }
    }
}
