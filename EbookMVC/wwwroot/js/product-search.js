$(document).ready(function() {
    let searchTimeout;
    const searchInput = $('#searchInput');
    const searchBtn = $('#searchBtn');
    const searchLoading = $('#searchLoading');
    const tableBody = $('table tbody');
    
    // Tìm kiếm real-time khi người dùng nhập
    searchInput.on('input', function() {
        clearTimeout(searchTimeout);
        const keyword = $(this).val().trim();
        
        // Debounce để tránh gọi API quá nhiều
        searchTimeout = setTimeout(function() {
            if (keyword.length > 0) {
                performSearch(keyword);
            } else {
                // Nếu không có từ khóa, reload trang để hiển thị tất cả sản phẩm
                window.location.href = '/Product';
            }
        }, 300); // Đợi 300ms sau khi người dùng ngừng nhập
    });
    
    // Xử lý phím Enter
    searchInput.on('keypress', function(e) {
        if (e.which === 13) { // Enter key
            e.preventDefault();
            const keyword = $(this).val().trim();
            if (keyword.length > 0) {
                performSearch(keyword);
            }
        }
    });
    
    // Xử lý click nút tìm kiếm
    searchBtn.on('click', function() {
        const keyword = searchInput.val().trim();
        if (keyword.length > 0) {
            performSearch(keyword);
        } else {
            // Nếu không có từ khóa, hiển thị tất cả sản phẩm
            window.location.href = '/Product';
        }
    });
    
    // Hàm thực hiện tìm kiếm
    function performSearch(keyword) {
        searchLoading.show();
        
        $.ajax({
            url: '/api/ProductApi/search',
            type: 'GET',
            data: { keyword: keyword },
            success: function(response) {
                searchLoading.hide();
                if (response.success) {
                    updateTable(response.data);
                } else {
                    updateTable([]);
                }
            },
            error: function() {
                searchLoading.hide();
                alert('Có lỗi xảy ra khi tìm kiếm. Vui lòng thử lại.');
            }
        });
    }
    
    // Hàm cập nhật bảng kết quả
    function updateTable(products) {
        tableBody.empty();
        
        if (products.length === 0) {
            tableBody.append(`
                <tr>
                    <td colspan="6" class="text-center">Không tìm thấy khóa học nào phù hợp</td>
                </tr>
            `);
        } else {
            products.forEach(function(product) {
                const imageHtml = product.imageUrl ? 
                    `<img src="${product.imageUrl}" alt="${product.name}" style="width: 60px; height: 60px; object-fit: cover;" class="img-thumbnail" />` :
                    `<div class="bg-light d-flex align-items-center justify-content-center" style="width: 60px; height: 60px;">
                        <i class="fas fa-image text-muted"></i>
                    </div>`;
                
                tableBody.append(`
                    <tr>
                        <td>${product.id}</td>
                        <td>${imageHtml}</td>
                        <td>${product.name}</td>
                        <td>${product.price}</td>
                        <td>${product.categoryName}</td>
                        <td>
                            <div class="btn-group">
                                <a href="/Product/Details/${product.id}" class="btn btn-info btn-sm" title="Xem chi tiết">
                                    <i class="fas fa-eye"></i>
                                </a>
                                <a href="/Product/Edit/${product.id}" class="btn btn-warning btn-sm" title="Chỉnh sửa">
                                    <i class="fas fa-edit"></i>
                                </a>
                                <a href="/Product/Delete/${product.id}" class="btn btn-danger btn-sm" title="Xóa">
                                    <i class="fas fa-trash"></i>
                                </a>
                            </div>
                        </td>
                    </tr>
                `);
            });
        }
    }
});
