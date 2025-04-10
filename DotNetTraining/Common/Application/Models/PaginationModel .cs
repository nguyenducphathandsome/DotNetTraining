namespace DotNetTraining.Common.Application.Models
{
    public class PaginationModel
    {
        public int PageNumber { get; set; }  // Trang hiện tại
        public int PageSize { get; set; }    // Số lượng bản ghi trên mỗi trang
        public int TotalItems { get; set; }  // Tổng số bản ghi
        public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);  // Tổng số trang

        public PaginationModel() { }

        public PaginationModel(int pageNumber, int pageSize, int totalItems)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalItems = totalItems;
        }
    }

}
