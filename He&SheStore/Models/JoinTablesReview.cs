namespace He_SheStore.Models
{
    public class JoinTablesReview
    {
        public Product Product { get; set; }
        public Review Review { get; set; }
        public Customer Customer { get; set; }
        public Category Category { get; set; }
    }

}
