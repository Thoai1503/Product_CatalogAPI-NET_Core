-- ============================================================
-- Catalog_ElectricStoreDB - MySQL Syntax
-- Converted from SQL Server (MSSQL) script
-- ============================================================

CREATE DATABASE IF NOT EXISTS `Catalog_ElectricStoreDB`
  CHARACTER SET utf8mb4
  COLLATE utf8mb4_unicode_ci;

USE `Catalog_ElectricStoreDB`;

-- ============================================================
-- TABLE: __EFMigrationsHistory
-- ============================================================
CREATE TABLE `__EFMigrationsHistory` (
  `MigrationId` VARCHAR(150) NOT NULL,
  `ProductVersion` VARCHAR(32) NOT NULL,
  PRIMARY KEY (`MigrationId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ============================================================
-- TABLE: attributes
-- ============================================================
CREATE TABLE `attributes` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `name` NVARCHAR(255) NOT NULL,
  `slug` NVARCHAR(255) NULL,
  `data_type` NVARCHAR(50) NOT NULL,
  `unit` NVARCHAR(50) NOT NULL,
  `status` INT NOT NULL DEFAULT 1,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ============================================================
-- TABLE: attribute_value
-- ============================================================
CREATE TABLE `attribute_value` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `attribute_id` INT NOT NULL,
  `value` NCHAR(50) NOT NULL,
  PRIMARY KEY (`id`),
  CONSTRAINT `FK_attribute_value_attributes`
    FOREIGN KEY (`attribute_id`) REFERENCES `attributes` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ============================================================
-- TABLE: brands
-- ============================================================
CREATE TABLE `brands` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `name` NVARCHAR(50) NOT NULL,
  `slug` VARCHAR(50) NOT NULL DEFAULT 'default-slug',
  `status` INT NOT NULL DEFAULT 1,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ============================================================
-- TABLE: categories
-- ============================================================
CREATE TABLE `categories` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `name` NVARCHAR(120) NOT NULL,
  `slug` NVARCHAR(140) NOT NULL,
  `parent_id` INT NULL,
  `path` NVARCHAR(400) NULL,
  `level` INT NOT NULL DEFAULT 0,
  `status` INT NOT NULL DEFAULT 1,
  `created_at` DATETIME(6) NOT NULL DEFAULT (UTC_TIMESTAMP(6)),
  PRIMARY KEY (`id`),
  UNIQUE KEY `UQ_categories_slug` (`slug`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ============================================================
-- TABLE: category_attributes
-- ============================================================
CREATE TABLE `category_attributes` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `category_id` INT NOT NULL,
  `attribute_id` INT NOT NULL,
  `is_filterable` TINYINT(1) NOT NULL DEFAULT 0,
  `is_variant_level` TINYINT(1) NOT NULL DEFAULT 0,
  `is_required` TINYINT(1) NOT NULL DEFAULT 0,
  PRIMARY KEY (`id`),
  CONSTRAINT `FK_category_attributes_categories`
    FOREIGN KEY (`category_id`) REFERENCES `categories` (`id`),
  CONSTRAINT `FK_category_attributes_attributes`
    FOREIGN KEY (`attribute_id`) REFERENCES `attributes` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ============================================================
-- TABLE: category_brands
-- ============================================================
CREATE TABLE `category_brands` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `brand_id` INT NOT NULL,
  `category_id` INT NOT NULL,
  PRIMARY KEY (`id`),
  CONSTRAINT `FK_category_brands_brands`
    FOREIGN KEY (`brand_id`) REFERENCES `brands` (`id`),
  CONSTRAINT `FK_category_brands_categories`
    FOREIGN KEY (`category_id`) REFERENCES `categories` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ============================================================
-- TABLE: provinces
-- ============================================================
CREATE TABLE `provinces` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `name` NVARCHAR(100) NOT NULL,
  `code` VARCHAR(20) NOT NULL,
  `status` INT NOT NULL DEFAULT 1,
  PRIMARY KEY (`id`),
  UNIQUE KEY `UQ_provinces_code` (`code`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ============================================================
-- TABLE: districts
-- ============================================================
CREATE TABLE `districts` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `name` NVARCHAR(100) NOT NULL,
  `code` VARCHAR(20) NOT NULL,
  `province_id` INT NOT NULL,
  `status` INT NOT NULL DEFAULT 1,
  PRIMARY KEY (`id`),
  UNIQUE KEY `UQ_districts_code` (`code`),
  KEY `IX_districts_province_id` (`province_id`),
  CONSTRAINT `FK_districts_provinces`
    FOREIGN KEY (`province_id`) REFERENCES `provinces` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ============================================================
-- TABLE: wards
-- ============================================================
CREATE TABLE `wards` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `name` NVARCHAR(100) NOT NULL,
  `code` VARCHAR(20) NOT NULL,
  `district_id` INT NOT NULL,
  `status` INT NOT NULL DEFAULT 1,
  PRIMARY KEY (`id`),
  UNIQUE KEY `UQ_wards_code` (`code`),
  KEY `IX_wards_district_id` (`district_id`),
  CONSTRAINT `FK_wards_districts`
    FOREIGN KEY (`district_id`) REFERENCES `districts` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ============================================================
-- TABLE: users
-- ============================================================
CREATE TABLE `users` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `email` NCHAR(50) NOT NULL,
  `password` NCHAR(150) NOT NULL,
  `phone` NCHAR(11) NOT NULL,
  `full_name` NVARCHAR(50) NOT NULL,
  `role` INT NOT NULL,
  `status` INT NOT NULL,
  `created_at` TIMESTAMP NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ============================================================
-- TABLE: user_addresses
-- ============================================================
CREATE TABLE `user_addresses` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `user_id` INT NOT NULL,
  `full_name` NVARCHAR(100) NOT NULL,
  `phone` NVARCHAR(20) NOT NULL,
  `province_id` INT NOT NULL,
  `district_id` INT NOT NULL,
  `ward_id` INT NOT NULL,
  `address_detail` NVARCHAR(500) NOT NULL,
  `address_type` INT NOT NULL DEFAULT 1,
  `is_default` TINYINT(1) NOT NULL DEFAULT 0,
  `status` INT NOT NULL DEFAULT 1,
  `created_at` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` DATETIME NULL,
  PRIMARY KEY (`id`),
  KEY `IX_user_addresses_user_id` (`user_id`),
  CONSTRAINT `FK_user_addresses_users`
    FOREIGN KEY (`user_id`) REFERENCES `users` (`id`) ON DELETE CASCADE,
  CONSTRAINT `FK_user_addresses_provinces`
    FOREIGN KEY (`province_id`) REFERENCES `provinces` (`id`),
  CONSTRAINT `FK_user_addresses_districts`
    FOREIGN KEY (`district_id`) REFERENCES `districts` (`id`),
  CONSTRAINT `FK_user_addresses_wards`
    FOREIGN KEY (`ward_id`) REFERENCES `wards` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ============================================================
-- TABLE: orders
-- ============================================================
CREATE TABLE `orders` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `user_id` INT NOT NULL,
  `discount` DECIMAL(18,0) NOT NULL,
  `total` DECIMAL(18,0) NOT NULL,
  `created_at` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `status` INT NOT NULL DEFAULT 1,
  `address_id` INT NULL,
  PRIMARY KEY (`id`),
  CONSTRAINT `FK_orders_users`
    FOREIGN KEY (`user_id`) REFERENCES `users` (`id`),
  CONSTRAINT `FK_orders_user_addresses`
    FOREIGN KEY (`address_id`) REFERENCES `user_addresses` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ============================================================
-- TABLE: products
-- ============================================================
CREATE TABLE `products` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `name` NVARCHAR(255) NOT NULL,
  `slug` NCHAR(100) NOT NULL,
  `brand_id` INT NULL,
  `category_id` INT NOT NULL,
  `description` TEXT NOT NULL,
  `rating` DECIMAL(3,2) NOT NULL DEFAULT 0,
  `status` INT NOT NULL DEFAULT 1,
  `created_at` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  CONSTRAINT `FK_products_brands`
    FOREIGN KEY (`brand_id`) REFERENCES `brands` (`id`),
  CONSTRAINT `FK_products_categories`
    FOREIGN KEY (`category_id`) REFERENCES `categories` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ============================================================
-- TABLE: product_variants
-- ============================================================
CREATE TABLE `product_variants` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `name` NCHAR(250) NOT NULL DEFAULT 'default',
  `product_id` INT NOT NULL,
  `SKU` NCHAR(100) NOT NULL,
  `price` INT NOT NULL,
  `status` INT NOT NULL DEFAULT 1,
  `quantity` INT NOT NULL DEFAULT 1,
  `created_at` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  CONSTRAINT `FK_product_variants_products`
    FOREIGN KEY (`product_id`) REFERENCES `products` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ============================================================
-- TABLE: product_attribute
-- ============================================================
CREATE TABLE `product_attribute` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `product_id` INT NOT NULL,
  `attribute_id` INT NOT NULL,
  `value_text` NVARCHAR(50) NULL,
  `value_decimal` DECIMAL(18,0) NULL,
  `value_int` INT NULL,
  `attribute_value_id` INT NULL,
  PRIMARY KEY (`id`),
  CONSTRAINT `FK_product_attribute_products`
    FOREIGN KEY (`product_id`) REFERENCES `products` (`id`) ON DELETE CASCADE,
  CONSTRAINT `FK_product_attribute_attributes`
    FOREIGN KEY (`attribute_id`) REFERENCES `attributes` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ============================================================
-- TABLE: product_image
-- ============================================================
CREATE TABLE `product_image` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `product_id` INT NOT NULL,
  `variant_id` INT NOT NULL,
  `url` VARCHAR(250) NOT NULL,
  PRIMARY KEY (`id`),
  CONSTRAINT `FK_product_image_product_variants`
    FOREIGN KEY (`variant_id`) REFERENCES `product_variants` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ============================================================
-- TABLE: variant_attribute
-- ============================================================
CREATE TABLE `variant_attribute` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `attribute_id` INT NOT NULL,
  `variant_id` INT NOT NULL,
  `value_int` INT NULL,
  `value_decimal` DECIMAL(18,0) NULL,
  `value_text` NVARCHAR(50) NULL,
  `attribute_value_id` INT NULL,
  PRIMARY KEY (`id`),
  CONSTRAINT `FK_variant_attribute_product_variants`
    FOREIGN KEY (`variant_id`) REFERENCES `product_variants` (`id`),
  CONSTRAINT `FK_variant_attribute_attributes`
    FOREIGN KEY (`attribute_id`) REFERENCES `attributes` (`id`),
  CONSTRAINT `FK_variant_attribute_attribute_value`
    FOREIGN KEY (`attribute_value_id`) REFERENCES `attribute_value` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ============================================================
-- TABLE: cart
-- ============================================================
CREATE TABLE `cart` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `user_id` INT NOT NULL,
  `variant_id` INT NOT NULL,
  `quantity` INT NOT NULL,
  `unit_price` DECIMAL(18,0) NULL,
  PRIMARY KEY (`id`),
  CONSTRAINT `FK_cart_product_variants`
    FOREIGN KEY (`variant_id`) REFERENCES `product_variants` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ============================================================
-- TABLE: order_detail
-- ============================================================
CREATE TABLE `order_detail` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `order_id` INT NOT NULL,
  `variant_id` INT NOT NULL,
  `quantity` INT NOT NULL,
  PRIMARY KEY (`id`),
  CONSTRAINT `FK_order_detail_orders`
    FOREIGN KEY (`order_id`) REFERENCES `orders` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ============================================================
-- TABLE: Inventory
-- ============================================================
CREATE TABLE `Inventory` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `variant_id` INT NOT NULL,
  `available_quantity` INT NOT NULL,
  `reversed_quantity` INT NOT NULL,
  `update_at` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ============================================================
-- TABLE: InventoryTransaction
-- ============================================================
CREATE TABLE `InventoryTransaction` (
  `id` INT NOT NULL,
  `variant_id` INT NOT NULL,
  `change_quantity` INT NOT NULL,
  `reference_id` INT NOT NULL,
  `created_at` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ============================================================
-- DATA: attributes
-- ============================================================
INSERT INTO `attributes` (`id`, `name`, `slug`, `data_type`, `unit`, `status`) VALUES
(1, N'Màu sắc', 'mau-sac', 'nvarchar', '', 1),
(2, N'Dung lượng pin', 'dung-luong-pin', 'int', 'mAh', 1),
(3, N'Kích thước màn hình', 'kich-thuoc-man-hinh', 'decimal', 'inch', 1),
(4, N'Bộ nhớ trong', 'bo-nho-trong', 'int', 'GB', 1),
(5, 'RAM', 'ram', 'int', 'GB', 1),
(6, N'Serries CPU', 'cpu-type', 'nvarchar', '', 1),
(7, 'RAM', 'ram', 'int', 'GB', 1),
(8, N'Dung lượng ổ cứng', 'dung-luong-o-cung', 'int', 'GB', 1),
(9, N'Loại ổ cứng', 'loai-o-cung', 'nvarchar', '', 1),
(10, N'Hệ điều hành', 'he-dieu-hanh', 'nvarchar', '', 1),
(11, N'Loại bàn phím', 'loai-ban-phim', 'nvarchar', '', 1),
(12, N'Loại PC ', 'loai-pc', 'nvarchar', '', 1);

-- ============================================================
-- DATA: attribute_value
-- ============================================================
INSERT INTO `attribute_value` (`id`, `attribute_id`, `value`) VALUES
(1, 6, 'Core I5'),
(2, 6, 'Core I7'),
(3, 6, 'Core 5'),
(4, 6, 'Core 7'),
(5, 6, 'Core Ultra 5'),
(6, 7, '8'),
(7, 7, '16'),
(8, 7, '32'),
(9, 7, '64'),
(10, 7, '36'),
(11, 7, '24'),
(12, 7, '48'),
(13, 7, '128'),
(14, 5, '8'),
(15, 5, '16'),
(16, 5, '32'),
(17, 5, '64'),
(18, 5, '128'),
(19, 4, '256 GB'),
(20, 4, '516 GB'),
(21, 4, '1T'),
(22, 8, '256 GB'),
(23, 8, '516 GB'),
(24, 1, 'Cam'),
(25, 1, N'Đỏ'),
(26, 1, N'Đen'),
(27, 2, '3000 mah'),
(28, 3, '21 inch'),
(29, 9, 'SSD'),
(30, 10, 'Window'),
(31, 8, '1T'),
(32, 6, 'Core Ultra 7'),
(33, 11, N'Bàn phím cơ'),
(34, 11, N'Bàn phím giả cơ'),
(35, 11, N'Bàn phím thường');

-- ============================================================
-- DATA: brands
-- ============================================================
INSERT INTO `brands` (`id`, `name`, `slug`, `status`) VALUES
(1, 'Acer', 'acer', 1),
(4, 'Asus', 'asus', 1),
(5, 'Dell', 'dell', 1),
(6, 'HP', 'hp', 1),
(7, 'Samsung', 'samsung', 1),
(8, 'Apple', 'apple', 1),
(9, 'Sony', 'sony', 1),
(10, 'LG', 'lg', 1),
(11, 'Dell', 'dell2', 1),
(12, 'OPPO', 'oppo', 1),
(14, 'Lenovo', 'lenovo', 1),
(15, 'Apple', 'apple2', 1),
(16, 'LG', 'lg2', 1),
(17, 'Msi', 'msi', 1),
(18, 'Gigabyte', 'gigabyte', 1),
(19, 'Realme', 'realme', 1),
(20, 'Xiaomi', 'xiaomi', 1),
(21, 'Vivo', 'vivo', 1),
(22, 'Aula', 'aula', 1),
(23, 'Razor', 'razor', 1),
(24, 'Logitech', 'logitech', 1),
(25, 'Keychrone', 'keychrone', 1);

-- ============================================================
-- DATA: categories
-- ============================================================
INSERT INTO `categories` (`id`, `name`, `slug`, `parent_id`, `path`, `level`, `status`, `created_at`) VALUES
(11, N'Điện thoại', 'dien-thoai', 0, '/dien-thoai', 0, 1, '2025-08-26 14:41:01.093457'),
(12, 'Laptop', 'Laptop', 0, '/laptop', 0, 1, '2025-08-27 08:57:41.302392'),
(13, 'Laptop Gaming', 'laptop-gaming', 12, '/laptop/laptop-gaming', 1, 1, '2025-09-18 10:56:28.166418'),
(14, N'Linh kiện máy tính', 'linh-kien-may-tinh', 0, '/linh-kien-may-tinh', 0, 1, '2025-08-27 11:39:51.972976'),
(15, N'Ổ cứng', 'o-cung', 14, '/linh-kien-may-tinh/o-cung', 1, 1, '2025-08-27 11:43:58.663807'),
(16, N'Ổ cứng SSD', 'o-cung-ssd', 15, '/linh-kien-may-tinh/o-cung/o-cung-ssd', 2, 1, '2025-09-21 10:10:12.372555'),
(17, N'Ổ cứng HDD', 'o-cung-hdd', 15, '/linh-kien-may-tinh/o-cung/o-cung-hdd', 2, 1, '2025-09-02 17:59:17.380783'),
(112, N'Đồng hồ', 'dong-ho', 0, '/dong-ho', 0, 1, '2025-09-03 04:17:36.673856'),
(113, N'PC - Máy tính bàn', 'pc---may-tinh-ban', 0, '/pc---may-tinh-ban', 0, 1, '2026-01-07 03:45:20.532095'),
(115, N'Đồng hồ điện tử', 'dong-ho-dien-tu', 112, '/dong-ho/dong-ho-dien-tu', 1, 1, '2025-09-23 06:14:31.783904'),
(116, N'Đồng hồ cơ', 'dong-ho-co', 112, '/dong-ho/dong-ho-co', 1, 1, '2025-09-02 08:58:44.653683'),
(117, 'PC Gaming', 'pc-gaming', 113, '/pc---may-tinh-ban/pc-gaming', 1, 1, '2025-09-02 08:59:35.678180'),
(118, N'Phụ kiện máy tính', 'phu-kien-may-tinh', 0, '/phu-kien-may-tinh', 0, 1, '2026-01-07 03:43:48.762043'),
(119, N'Bàn phím', 'ban-phim', 118, '/phu-kien-may-tinh/ban-phim', 1, 1, '2026-01-07 03:44:41.718204');

-- ============================================================
-- DATA: category_attributes
-- ============================================================
INSERT INTO `category_attributes` (`id`, `category_id`, `attribute_id`, `is_filterable`, `is_variant_level`, `is_required`) VALUES
(4, 16, 8, 0, 0, 0),
(27, 14, 4, 0, 0, 0),
(29, 15, 9, 0, 0, 1),
(31, 15, 6, 0, 1, 1),
(39, 116, 2, 0, 0, 0),
(43, 116, 8, 0, 0, 0),
(54, 112, 1, 1, 1, 0),
(89, 12, 3, 1, 0, 1),
(90, 11, 2, 1, 0, 0),
(95, 12, 5, 0, 1, 1),
(96, 12, 8, 0, 1, 0),
(97, 12, 6, 0, 1, 0),
(98, 11, 5, 0, 1, 0),
(99, 119, 11, 1, 0, 1),
(100, 113, 12, 1, 0, 0),
(101, 113, 5, 1, 1, 0),
(102, 113, 8, 1, 1, 0),
(103, 113, 6, 1, 1, 0);

-- ============================================================
-- DATA: category_brands
-- ============================================================
INSERT INTO `category_brands` (`id`, `brand_id`, `category_id`) VALUES
(2, 4, 12),
(4, 6, 12),
(5, 8, 12),
(6, 11, 12),
(7, 17, 12),
(8, 18, 12),
(9, 7, 11),
(10, 8, 11),
(11, 9, 11),
(12, 10, 11),
(13, 12, 11),
(14, 19, 11),
(15, 20, 11),
(16, 23, 119),
(17, 24, 119),
(18, 25, 119),
(20, 22, 119);

-- ============================================================
-- DATA: provinces
-- ============================================================
INSERT INTO `provinces` (`id`, `name`, `code`, `status`) VALUES
(1, N'Thành phố Hồ Chí Minh', 'HCM', 1),
(2, N'Hà Nội', 'HN', 1),
(3, N'Đà Nẵng', 'DN', 1);

-- ============================================================
-- DATA: districts
-- ============================================================
INSERT INTO `districts` (`id`, `name`, `code`, `province_id`, `status`) VALUES
(1, N'Quận 1', 'Q1', 1, 1),
(2, N'Quận 3', 'Q3', 1, 1),
(3, N'Quận 10', 'Q10', 1, 1),
(4, N'Quận Bình Thạnh', 'BT', 1, 1),
(5, N'Quận Thủ Đức', 'TD', 1, 1);

-- ============================================================
-- DATA: wards
-- ============================================================
INSERT INTO `wards` (`id`, `name`, `code`, `district_id`, `status`) VALUES
(1, N'Phường Bến Nghé', 'BN', 1, 1),
(2, N'Phường Bến Thành', 'BTH', 1, 1),
(3, N'Phường 1', 'P1', 2, 1),
(4, N'Phường 2', 'P2', 2, 1),
(5, N'Phường 1', 'P1_Q10', 3, 1);

-- ============================================================
-- DATA: users
-- ============================================================
INSERT INTO `users` (`id`, `email`, `password`, `phone`, `full_name`, `role`, `status`) VALUES
(1, 'vothoai1503@gmail.com', '$2a$10$qY5w6RDTQ1rZDy.naiaflOsnD0jJ7nGKsi5icFiTonRG8h0opwx/6', '0862830787', 'Vo Giang Thoai', 1, 1),
(2, 'john.tv350@gmail.com', '$2a$10$qY5w6RDTQ1rZDy.naiaflOsnD0jJ7nGKsi5icFiTonRG8h0opwx/6', '0986625315', N'Lý Tiểu Long', 2, 1),
(3, 'lytieulong@gmail.com', '$2b$10$TDiJnNa3E2tKm8WTfxpg2ubojlJi4GLbLPl.q.YlqaTt1sUYak52a', '034354', 'Ly Tieu Long', 0, 1),
(4, 'user3@gmail.com', '$2b$10$go0mQVBijELFW2JNKgIQoODXd9DB0a4vJu/JdmMi6J9vV17UdwK0m', '034354', 'User 3', 0, 1),
(5, 'user4@gmail.com', '$2b$10$b8Utas2LiKaBXiYstU8PDu4xLMig4AQ97pJmXYTv.WfexWQ/Db96q', '034354', 'User 4', 0, 1),
(6, 'user5@gmail.com', '$2b$10$VCe3Dr5JXVI3YNrPNcX5DuhBm6mE3m702SgUsqMkVurNapTxFCsuS', '034354', 'User 5', 0, 1),
(7, 'user6@gmail.com', '$2b$10$5ktTrNxuQNu6BKMpMOUuQOSufxIxjq/v6yxx0l77Akmpe558crUNa', '034354', 'user 6', 0, 1),
(8, 'user7@gmail.com', '$2b$10$71zjElV0PEMZEzluT/Z9jOL3MgyxTJAjUUxNDwcSWSLtVuS6soExy', '034354', 'user 7', 0, 1),
(9, 'tinhvomon@gmail.com', '$2b$10$p/sAaLWfT.NMQ3YUWGrlU.u.LhCj05Vzba2nAuJHmb.UCklbBh./y', '034354', 'Tinh Vo Mon', 0, 1),
(10, 'manhlongquagiang@gmail.com', '$2b$10$TOyDb7SR26HtJlNBbzGc7uOe2peHFa2VJ5pA.mzObvCBYR8yohoCW', '034354', 'Manh Long Qua Giang', 0, 1),
(11, 'user100@gmail.com', '$2b$10$ELbqt2/ivbVEpdTaCjLpZuImHpxp5NiGRywm95RvwP7FExkL4g9Ku', '034354', 'User 100', 0, 1),
(12, 'user99@gmail.com', '$2b$10$ZKiTsFc.az6hKNq..1Hj8.Z7zQjZ10ZWXwZJDluuFrPMY0Az2xazu', '034354', 'User 99', 0, 1),
(13, 'user98@gmail.com', '$2b$10$/ZQSWpaEv8Zne3ZbQgEakuaukH7z.xN1PWAZIiGlS7Nowc5Rel4tS', '034354', 'User 98', 0, 1),
(14, 'user97@gmail.com', '$2b$10$Re6BNvyeAkBTAvEn9CwiwueLdSTjp3LhfRTO4VDI4hdDeI6y2v5QC', '034354', 'User 97', 0, 1),
(15, 'user96@gmail.com', '$2b$10$CB7lmJ8C2oxgxWqvUkDJ6.bpt5QG9MGkJq4HjBBDBEjIUegs62Z/C', '034354', 'User 96', 0, 1),
(16, 'user95@gmail.com', '$2b$10$mWHoup0yuU.QGJiQWOM56u9Dv7u3.7Ctv2qkmUnhhNUxqtRhZe7qi', '034354', 'User 95', 0, 1),
(17, 'user94@gmail.com', '$2b$10$eE.6TBRPrWQ69jso0xDISeY53b87oAl2qPaKSynr.mbNfuEMStVVG', '034354', 'User 94', 0, 1),
(18, 'user93@gmail.com', '$2b$10$3M5Kr2HcybN26mnIV1CuWOI/IYAAC4z5w/C6dh.mRDAZvvZWyQ5kW', '034354', 'User 93', 0, 1),
(19, 'user92@gmail.com', '$2b$10$w0xL3FqO4It4tcNWqRvS.OqPVARAK5tLXLQF3bWthcwQ4uP3K21N2', '034354', 'User 92', 0, 1),
(20, 'user91@gmail.com', '$2b$10$92Vm.DOAKoeQZ52dFRqzTuFLYPZtyiQgtrM8xMkU/Gybpb0ohz8jq', '034354', 'User 91', 0, 1),
(21, 'user90@gmail.com', '$2b$10$uWl3czmR2VzEuIAUETu6p.rqMsx9nEYj67OlL2vbBR/3rxeBAHJ72', '034354', 'user 90', 0, 1),
(22, 'user89@gmail.com', '$2b$10$dfjPvmYaWGUoWSPRvECXLurT1rYAWMXVQVhzd5OkHbjs07aN4dPi6', '034354', 'user 89', 0, 1),
(23, 'user88@gmail.com', '$2b$10$Hwc.mVjscwg5jMxXRm2kHOcjZkrK7von1AIOFw56qPgLmP3bVNtua', '034354', 'user 88', 0, 1),
(24, 'user87@gmail.com', '$2b$10$n3d27ylE3fOovXNBEWXN5eosFXh.098wUgxGzQYjqQJszaZwXlgoO', '034354', 'user 87', 0, 1),
(25, 'user86@gmail.com', '$2b$10$Ee/8lE6e0e9qXqvev3KSeuyMkFx0JIjoWlUIn62YQjOqi3HULxVBS', '034354', 'User 86', 0, 1),
(26, 'user85@gmail.com', '$2b$10$1X8mQN7eENR00TpOOBIXXegv2ROJOjVK0b7O6LXQIatOfczEq3k.2', '034354', 'User 85', 0, 1),
(27, 'user84@gmail.com', '$2b$10$jfWtyu2Do/gx5iYhrpUFp.aco3UDjkW8gFqclKDoUEQNfn.5dbWNe', '034354', 'User 84', 0, 1),
(28, 'user83@gmail.com', '$2b$10$8m5dDW2nScdm33ltjM9m8e6ESoTktLlpd6nx4NbQNcFKVA91rO6iO', '034354', 'User 83', 0, 1),
(29, 'user81@gmail.com', '$2b$10$wcSwMUQhSKVB4FyroxzWZeNK0KNPM2pjq1WL19tMLUtD1uUCOIPg6', '034354', 'User 81', 0, 1),
(30, 'user80@gmail.com', '$2b$10$qbxeyZLBfYU9GeKBUIWKGedJ5s.ifani8G4Yt/VyVaOOxr.Bfia36', '034354', 'User 80', 0, 1),
(31, 'user79@gmail.com', '$2b$10$Ho3ooHJ6xI7xeUoWRIFx..DLWFphUoUiPXCHJivYpZMUhVLxHh8vG', '034354', 'User 79', 0, 1),
(32, 'user78@gmail.com', '$2b$10$it3n5l.yxA9RjucrGjG1q.YEsOUxGi/SJ44xOneY9B1mfW1xXncba', '034354', 'User 78', 0, 1),
(33, 'user77@gmail.com', '$2b$10$XhfqRWDq51W539cjDXVEU.cfCYPPgzpiyKGP7HHjmyJx9s8I44PwS', '034354', 'User 77', 0, 1),
(34, 'user76@gmail.com', '$2b$10$6JPTZSUwk.QfhJADX3283.HY4gV8OYEtxf8KytXXqiUjVm3DExIkG', '034354', 'User 76', 0, 1),
(35, 'user101@gmail.com', '$2b$10$wbt1./umm0nG/5p704yRL.5.LlIDAoj1E0y6KWZ4DB6ND4FJ1bWVi', '034354', 'user 101', 0, 1),
(36, 'user102@gmail.com', '$2b$10$6v1gzRNw59uakPEgT971K.C08hlBIzsFZ3PgN4HqdCX46B0APElzy', '034354', 'user 102', 0, 1),
(37, 'user103@gmail.com', '$2b$10$BfcQcOFbOsNHkSbErUeOqeBu8561ToISBIchqZdhvPAei551buSDC', '034354', 'user 103', 0, 1),
(38, 'user104@gmail.com', '$2b$10$IAP1gditsTQ/Bwvm8oRkGu7yqpB8qYqmzwdr1vygmiV.UuLTmGhRe', '034354', 'user 104', 0, 1),
(39, 'user105@gmail.com', '$2b$10$ddQCD9qT3qkMxoHTccF0muotEHd40GDwqrL3TBCUOOgZMOOfpdn3q', '034354', 'user 105', 0, 1),
(40, 'user106@gmail.com', '$2b$10$/ZNBpPHVff9PYm0KyT/k3esoud2VoNXZ8En3xSkQjDU/5uBpomNo2', '034354', 'user 106', 0, 1);

-- ============================================================
-- DATA: user_addresses
-- ============================================================
INSERT INTO `user_addresses` (`id`, `user_id`, `full_name`, `phone`, `province_id`, `district_id`, `ward_id`, `address_detail`, `address_type`, `is_default`, `status`, `created_at`, `updated_at`) VALUES
(3, 2, 'Vo Thoai', '0938662662', 1, 2, 1, '3333 Wall', 1, 1, 1, '2025-10-17 07:44:49', '2026-01-05 11:06:29'),
(14, 2, 'Thoai Giang Vo', '0862830787', 1, 2, 4, '351 Thanh thai', 1, 0, 1, '2025-10-18 15:30:15', '2026-01-05 11:04:31'),
(15, 2, 'Tinh Vo Mon', '0862830787', 1, 2, 3, '111 Ly Tieu Long', 1, 0, 1, '2025-10-19 10:48:39', '2026-01-05 11:06:25'),
(16, 2, N'Thoai Thiên Long', '0976111111', 1, 1, 1, N'3 Đường 3/2', 1, 0, 1, '2025-10-24 15:55:22', '2025-11-27 10:44:58'),
(17, 2, N'Thoai Thiên Long', '0976111111', 1, 1, 1, N'3 Đường 3/2', 1, 0, 1, '2025-10-25 03:11:07', '2025-11-27 10:44:42'),
(18, 2, N'Thoai Thiên Long', '0976111111', 1, 1, 1, N'3 Đường 3/2', 1, 0, 1, '2025-10-25 03:16:45', '2025-11-27 10:46:31'),
(19, 2, N'Thoai Thiên Long', '0976111111', 1, 1, 1, N'3 Đường 3/2', 1, 0, 1, '2025-10-25 03:21:22', '2025-11-27 10:45:06'),
(20, 3, 'Ly Tieu Long', '0390458433', 1, 2, 3, '657 Ham Tu', 1, 1, 1, '2025-10-27 15:06:08', '2025-11-25 07:41:16'),
(21, 3, 'Ly Tieu Long', '0390458433', 1, 2, 3, '657 Ham Tu', 1, 0, 1, '2025-10-27 15:06:08', '2025-11-25 07:41:13'),
(22, 8, 'User 7 123', '0922328289', 1, 2, 3, 'Ngo 1 Vach 3', 1, 1, 1, '2025-10-28 03:43:43', '2025-10-28 03:43:57'),
(23, 6, 'Thoai Giang Vo', '0862830787', 1, 3, 5, '32 3 thang 2', 1, 1, 1, '2025-10-28 05:55:08', '2025-10-28 05:55:16'),
(24, 4, 'Ba user 3', '0979877656', 1, 2, 4, N'3 Trần Quốc Thảo', 1, 0, 1, '2025-10-28 14:45:33', '2025-11-08 03:48:47'),
(25, 5, '', '', 1, 2, 3, N'121 Bao Thanh Thiên', 1, 1, 1, '2025-10-28 15:06:56', '2025-10-28 15:07:03'),
(26, 1, '', '', 1, 1, 2, N'1 Lê Lợi', 1, 1, 1, '2025-10-30 09:06:52', '2025-10-30 09:06:59'),
(27, 10, 'Duong Huynh', '0453395673', 1, 3, 5, N'45 Tô Hiến Thành', 1, 1, 1, '2025-10-31 09:39:43', '2025-10-31 09:39:48'),
(28, 4, 'Thoai Giang Vo', '0862830787', 1, 2, 4, '111 Ly Tieu Long', 1, 1, 1, '2025-11-01 13:00:12', '2025-11-08 04:02:45'),
(29, 22, 'Thoai Giang Vo', '0123654875', 1, 2, 4, '351 Thanh thai', 1, 1, 1, '2025-11-06 12:15:28', '2025-11-06 12:15:35'),
(30, 29, 'Thoai Giang Vo 2', '0862831111', 1, 1, 1, 'Ngo 1 Vach 3', 1, 1, 1, '2025-11-06 12:44:48', '2025-11-06 12:44:51'),
(31, 2, N'Thoai Thiên Long', '0976111111', 1, 1, 1, N'3 Đường 3/2', 1, 0, 1, '2025-11-09 00:20:23', '2025-11-27 10:44:37'),
(32, 36, 'Thoai Giang Vo', '0862830787', 1, 2, 3, '351 Thanh thai 345', 1, 1, 1, '2025-11-12 03:40:22', '2025-11-12 03:56:05'),
(33, 37, 'Bruce Lee', '0931311111', 1, 1, 2, N'2 Nguyễn Trung Trực', 1, 1, 1, '2025-11-12 04:02:02', '2025-11-12 04:02:13'),
(34, 38, 'Messi', '0876457123', 1, 2, 4, N'4 Trần Quang Diệu', 1, 1, 1, '2025-11-12 04:09:01', '2025-11-12 04:09:10'),
(35, 40, 'Thoai Giang Vo 123', '0862830787', 1, 2, 3, '12 Hoang Sa', 1, 1, 1, '2025-11-12 04:30:51', '2025-11-12 04:31:04');

-- ============================================================
-- DATA: products
-- ============================================================
INSERT INTO `products` (`id`, `name`, `slug`, `brand_id`, `category_id`, `description`, `rating`, `status`, `created_at`) VALUES
(5, 'Laptop Asus Vivobook Go 15', 'laptop-asus-vivobook-go-15', 4, 12, '', 0.00, 1, '2025-09-18 10:51:17'),
(8, 'Samsung Galaxy A06', 'samsung-galaxy-a06', 7, 11, 'string', 0.00, 1, '2025-09-20 22:55:52'),
(10, 'Samsung Galaxy S25 Ultra 5G ', 'samsung-galaxy-s25-ultra-5g', 7, 11, 'string', 0.00, 1, '2025-09-20 23:14:50'),
(11, 'iPhone 16 Pro Max 256GB', 'iphone-16-pro-max-256gb', 8, 11, 'string', 0.00, 1, '2025-09-20 23:19:43'),
(15, 'iPhone 17 Pro Max', 'iphone-17-pro-max', 8, 11, 'string', 0.00, 1, '2025-09-21 00:58:13'),
(17, 'Samsung Galaxy S24 5G', 'samsung-galaxy-s24-5g', 7, 11, '', 0.00, 1, '2025-09-21 04:37:45'),
(18, 'OPPO A5 8GB/128GB', 'oppo-a5-8gb128gb', 12, 11, '', 0.00, 1, '2025-09-21 04:41:05'),
(19, 'OPPO Reno14 F 5G 12GB/256GB', 'oppo-reno14-f-5g-12gb256gb', 12, 11, '', 0.00, 1, '2025-09-21 04:42:17'),
(20, 'OPPO Find N5 5G 16GB/512GB', 'oppo-find-n5-5g-16gb512gb', 12, 11, '', 0.00, 1, '2025-09-21 04:47:37'),
(21, 'OPPO Reno13 5G 12GB/256GB', 'oppo-reno13-5g-12gb256gb', 12, 11, '', 0.00, 1, '2025-09-21 04:50:26'),
(22, 'OPPO A5i Pro 8GB/128GB', 'oppo-a5i-pro-8gb128gb', 12, 11, '', 0.00, 1, '2025-09-21 04:53:51'),
(23, 'OPPO Reno12 F 8GB/256GB', 'oppo-reno12-f-8gb256gb', 12, 11, '', 0.00, 1, '2025-09-21 04:57:26'),
(26, 'Laptop Dell Inspiron 15 3520 - 25P231', 'laptop-dell-inspiron-15-3520---25p231', 11, 12, '', 0.00, 1, '2025-09-21 10:03:12'),
(27, 'Iphone 15 pro max', 'iphone-15-pro-max', 8, 11, '', 0.00, 1, '2025-09-21 10:04:52'),
(28, N'Điện thoại B', 'dien-thoai-b', 7, 11, '', 0.00, 1, '2025-09-21 22:37:41'),
(37, 'Generic H2 (Embedded)', 'generic-h2-embedded', 12, 11, '', 0.00, 1, '2025-09-25 21:31:09'),
(38, 'Acer Aspire Lite 15', 'acer-aspire-lite-15', 1, 12, '', 0.00, 1, '2025-11-08 12:01:20'),
(39, 'HP 14 ep1005TU - 9Z2W0PA', 'hp-14-ep1005tu---9z2w0pa', 6, 12, '', 0.00, 1, '2025-11-08 05:08:32'),
(40, 'Acer Aspire Lite 16', 'acer-aspire-lite-16', 1, 12, '', 0.00, 1, '2025-11-08 05:46:25'),
(41, 'Msi Prestige 13', 'msi-prestige-13', 17, 12, '', 0.00, 1, '2025-11-10 10:16:50'),
(42, N'Bàn phím cơ Gaming có dây AULA AG60 PRO (Xám)', 'ban-phim-co-gaming-co-day-aula-ag60-pro-xam', 22, 119, '', 0.00, 1, '2026-01-07 03:50:15'),
(43, N'Bàn phím cơ Gaming có dây Razer Huntsman V3 Pro', 'ban-phim-co-gaming-co-day-razer-huntsman-v3-pro', 23, 119, '', 0.00, 1, '2026-01-07 03:58:19'),
(44, N'Bàn phím giả cơ Logitech Gaming G213 (Đen)', 'ban-phim-gia-co-logitech-gaming-g213-den', 24, 119, '', 0.00, 1, '2026-01-07 19:12:47'),
(45, N'Bàn phím không dây Keychron B6P-K1 B6 Pro Space Gray', 'ban-phim-khong-day-keychron-b6p-k1-b6-pro-space-gray', 25, 119, '', 0.00, 1, '2026-01-07 19:49:30'),
(46, 'PC Dell Pro Tower Plus QBT1250 42PROU7QBT1250 (Intel Core Ultra 7-265/ 8GB DDR5/ 512GB SSD/ Windows 11)', 'pc-dell-pro-tower-plus-qbt1250-42prou7qbt1250-intel-core-ultra-7-265-8gb-ddr5-512gb-ssd-windows-11', 11, 113, '', 0.00, 1, '2026-01-07 23:15:14');

-- ============================================================
-- DATA: product_variants
-- ============================================================
INSERT INTO `product_variants` (`id`, `name`, `product_id`, `SKU`, `price`, `status`, `quantity`, `created_at`) VALUES
(4, 'Samsung Galaxy A06 4GB/128GB', 8, 'string', 25000000, 1, 1, '2025-09-23 14:09:53'),
(5, 'Samsung Galaxy A06 6GB/128GB', 8, 'string123', 27000000, 1, 1, '2025-09-23 14:12:13'),
(6, 'Samsung Galaxy S25 Ultra', 10, 'string999', 27500000, 1, 1, '2025-09-23 14:15:18'),
(9, 'Dell Inspiron 15 3530 - N5I5530W1', 26, 'ip8-8gb-125g', 12000000, 1, 1, '2025-09-24 23:01:47'),
(10, 'Dell Inspiron 15 3520 - 25P231', 26, 'ip8-16gb-256g', 19000000, 1, 1, '2025-09-24 23:05:59'),
(13, 'Asus Vivobook Go 15 E1504FA - NJ776W', 5, 'ip8-16gb-256g', 19000000, 1, 1, '2025-09-25 07:42:04'),
(14, 'Asus Vivobook Go 15 E1504FA R5 - NJ630W', 5, 'ip8-16gb-512g', 21000000, 1, 1, '2025-09-25 07:43:33'),
(15, 'Iphone 15 pro max', 27, 'ip15pm-16gb-256g', 29000000, 1, 1, '2025-09-25 10:42:13'),
(19, N'iPhone 17 Pro Max 256GB Vàng', 15, 'ip17-pmx-256-Gold', 37000000, 1, 1, '2025-09-25 23:38:14'),
(20, 'iPhone 17 Pro Max 512GB', 15, 'ip17-pmx-512', 41000000, 1, 1, '2025-09-25 23:43:22'),
(21, 'Dell Inspiron 15 3530 - N5I7216W1', 26, 'dell-inspiron-15-3530-N5I7216W1', 20990000, 1, 1, '2025-09-29 22:41:43'),
(22, 'Dell Inspiron 15 3530 - P16WD', 26, 'dell-inspiron-15-3530-p16wd', 21990000, 1, 1, '2025-09-29 22:52:01'),
(23, 'Asus Vivobook Go 15 E1504FA-BQ1150W', 5, 'Asus-Vivobook-Go-15-E1504FA-BQ1150W', 11990000, 1, 1, '2025-11-04 19:32:51'),
(26, 'Acer Aspire Lite 15 51M 5542 i5 1155G7', 38, 'NX.KS5SV.001', 12199000, 1, 1, '2025-11-08 12:05:03'),
(27, 'Acer Aspire Lite 15 51M 55NB i5 1155G7', 38, 'NX.KRSSV.001', 11999000, 1, 1, '2025-11-07 23:10:38'),
(28, 'HP 14 ep1005TU - 9Z2W0PA', 39, 'hp-14-ep1005TU-9Z2W0PA', 21990000, 1, 1, '2025-11-08 05:12:40'),
(29, 'HP 14-ep1007TU - 9Z2W1PA', 39, 'HP-14-ep1007TU-9Z2W1PA', 20590000, 1, 1, '2025-11-08 05:18:26'),
(30, 'Acer Aspire Lite 16 AI AL16-71P-5674', 40, 'acer-aspire-lite-16-AI-AL16-71P-5674', 16490000, 1, 1, '2025-11-08 05:48:09'),
(31, 'Msi Prestige 13 AI+ Evo A2VMG-040VN', 41, '241202760', 36490000, 1, 1, '2025-11-10 10:19:20'),
(32, 'Msi Prestige 13 AI Evo A1MG-062VN', 41, '240300078', 31990000, 1, 1, '2025-11-10 10:20:52'),
(33, N'Bàn phím cơ Gaming có dây AULA AG60 PRO (Xám)', 42, 'aula-ag60-pro', 3650000, 1, 1, '2026-01-07 03:50:51'),
(34, N'Bàn phím cơ Gaming có dây Razer Huntsman V3 Pro', 43, 'razor-hutsman-v3-pro', 5990000, 1, 1, '2026-01-07 03:59:26'),
(35, N'Bàn phím giả cơ Logitech Gaming G213 (Đen)', 44, 'logitech-gaming-g213', 979000, 1, 1, '2026-01-07 19:13:44'),
(37, 'n B6P-K1 B6 Pro Space Gray', 45, 'B6P-K1-B6-Pro-Space-Gray', 999000, 1, 1, '2026-01-07 19:51:58'),
(38, 'PC Dell Pro Tower Plus QBT1250 42PROU7QBT1250 (Intel Core Ultra 7-265/ 8GB DDR5/ 512GB SSD/ Windows 11)', 46, 'ip8-8gb-125g', 29390000, 1, 1, '2026-01-07 23:16:43');

-- ============================================================
-- DATA: orders
-- ============================================================
INSERT INTO `orders` (`id`, `user_id`, `discount`, `total`, `created_at`, `status`, `address_id`) VALUES
(12, 2, 0, 94500000, '2025-10-23 05:22:11', 2, 14),
(13, 2, 0, 38000000, '2025-10-23 05:31:58', 2, 15),
(14, 2, 0, 38000000, '2025-10-23 05:33:22', 2, 15),
(15, 2, 0, 38000000, '2025-10-23 05:37:47', 2, 15),
(16, 2, 0, 38000000, '2025-10-23 05:42:57', 2, 14),
(17, 2, 0, 152000000, '2025-10-23 06:31:03', 2, 14),
(18, 2, 0, 89500000, '2025-10-23 11:17:33', 2, 14),
(19, 2, 0, 94500000, '2025-10-23 15:39:48', 2, 15),
(20, 2, 0, 178990000, '2025-10-24 10:45:03', 2, 14),
(21, 2, 0, 269490000, '2025-10-25 12:41:15', 2, 15),
(22, 3, 0, 58000000, '2025-10-27 15:06:45', 2, 20),
(23, 8, 0, 50000000, '2025-10-28 03:44:54', 2, 22),
(24, 6, 0, 46500000, '2025-10-28 05:55:50', 2, 23),
(25, 4, 0, 95980000, '2025-10-28 14:47:56', 2, 24),
(26, 5, 0, 119000000, '2025-10-28 15:09:58', 2, 25),
(27, 5, 0, 58000000, '2025-10-28 15:10:42', 2, 25),
(28, 4, 0, 31000000, '2025-10-28 15:13:06', 2, 24),
(29, 4, 0, 93000000, '2025-10-29 07:36:37', 2, 24),
(30, 2, 0, 179000000, '2025-10-29 14:08:29', 2, 15),
(31, 2, 0, 57000000, '2025-10-30 03:21:23', 2, 15),
(32, 1, 0, 243000000, '2025-10-30 09:07:51', 2, 26),
(33, 10, 0, 55000000, '2025-10-31 09:40:05', 2, 27),
(34, 10, 0, 105000000, '2025-10-31 11:09:05', 2, 27),
(35, 4, 0, 156990000, '2025-11-01 12:38:32', 2, 24),
(36, 4, 0, 67500000, '2025-11-01 12:54:48', 2, 24),
(37, 5, 0, 85000000, '2025-11-03 01:42:43', 2, 25),
(38, 5, 0, 40000000, '2025-11-03 01:43:22', 2, 25),
(39, 2, 0, 97000000, '2025-11-03 15:50:20', 2, 16),
(40, 2, 0, 91500000, '2025-11-04 01:37:46', 2, 18),
(41, 2, 0, 75500000, '2025-11-04 02:19:00', 2, 19),
(42, 1, 0, 190000000, '2025-11-04 07:08:59', 2, 26),
(43, 22, 0, 118000000, '2025-11-06 12:15:57', 2, 29),
(44, 29, 0, 38000000, '2025-11-06 12:45:07', 2, 30),
(45, 3, 0, 75000000, '2025-11-06 14:25:06', 2, 20),
(46, 3, 0, 76000000, '2025-11-06 15:28:40', 2, 21),
(47, 3, 0, 116500000, '2025-11-07 16:20:23', 2, 20),
(48, 4, 0, 59000000, '2025-11-08 03:35:27', 2, 28),
(49, 4, 0, 54500000, '2025-11-08 03:50:13', 2, 24),
(50, 1, 0, 205990000, '2025-11-08 15:46:22', 2, 26),
(54, 40, 0, 33000000, '2025-11-12 06:11:17', 2, 35),
(55, 3, 0, 33000000, '2025-11-12 06:16:45', 2, 21),
(56, 40, 0, 40000000, '2025-11-12 06:19:05', 2, 35),
(57, 40, 0, 40000000, '2025-11-12 06:24:48', 2, 35),
(58, 40, 0, 38000000, '2025-11-12 06:31:20', 3, 35),
(59, 40, 0, 33000000, '2025-11-12 06:41:53', 2, 35),
(60, 40, 0, 52000000, '2025-11-12 06:45:10', 2, 35),
(61, 40, 0, 33000000, '2025-11-12 06:50:43', 2, 35),
(62, 3, 0, 68500000, '2025-11-12 06:54:19', 2, 21),
(63, 3, 0, 164000000, '2025-11-12 06:59:03', 2, 21),
(64, 40, 0, 31000000, '2025-11-12 07:01:06', 2, 35),
(65, 40, 0, 41000000, '2025-11-12 07:06:55', 2, 35),
(70, 40, 0, 31000000, '2025-11-12 07:20:46', 2, 35),
(72, 2, 0, 31000000, '2025-11-12 07:24:24', 2, 14),
(73, 3, 0, 27500000, '2025-11-16 02:36:26', 2, 20),
(74, 3, 0, 27500000, '2025-11-16 02:36:39', 2, 20),
(75, 3, 0, 27500000, '2025-11-16 02:36:50', 2, 21),
(76, 2, 0, 84960000, '2025-11-16 02:49:45', 2, 18),
(77, 3, 0, 12000000, '2025-11-16 02:51:36', 2, 21),
(78, 2, 0, 40000000, '2025-11-23 11:06:25', 2, 18),
(79, 2, 0, 40000000, '2025-11-23 11:06:33', 2, 16),
(80, 2, 0, 40000000, '2025-11-23 11:06:49', 2, 17),
(81, 2, 0, 12000000, '2025-11-23 11:11:12', 2, 17),
(82, 2, 0, 66000000, '2025-11-23 11:24:40', 2, 31),
(83, 2, 0, 19000000, '2025-11-23 11:28:40', 2, 14),
(84, 2, 0, 40000000, '2025-11-23 11:36:17', 2, 16),
(85, 2, 0, 39500000, '2025-11-23 11:38:52', 2, 16),
(86, 2, 0, 204490000, '2025-12-21 03:34:38', 2, 18),
(87, 2, 0, 27500000, '2026-01-05 10:45:28', 2, 15),
(88, 2, 0, 39000000, '2026-01-05 11:02:20', 2, 15),
(89, 2, 0, 27500000, '2026-01-05 11:07:09', 2, 3),
(90, 2, 0, 27500000, '2026-01-05 11:11:43', 2, 3),
(91, 2, 0, 27000000, '2026-01-05 11:14:23', 2, 3),
(92, 2, 0, 27000000, '2026-01-05 11:14:35', 2, 3),
(93, 2, 0, 27000000, '2026-01-05 11:14:52', 2, 3),
(94, 2, 0, 12000000, '2026-01-05 12:02:59', 2, 3),
(95, 2, 0, 12000000, '2026-01-05 12:06:40', 2, 3);

-- ============================================================
-- DATA: order_detail
-- ============================================================
INSERT INTO `order_detail` (`id`, `order_id`, `variant_id`, `quantity`) VALUES
(26, 12, 9, 1),(27, 12, 6, 3),(28, 13, 10, 1),(29, 13, 13, 1),(30, 14, 13, 1),
(31, 14, 10, 1),(32, 15, 10, 1),(33, 15, 13, 1),(34, 16, 10, 1),(35, 16, 13, 1),
(36, 17, 9, 6),(37, 17, 10, 1),(38, 17, 14, 2),(39, 17, 13, 1),(40, 18, 9, 2),
(41, 18, 6, 1),(42, 18, 10, 2),(43, 19, 9, 4),(44, 19, 10, 1),(45, 19, 6, 1),
(46, 20, 9, 3),(47, 20, 21, 1),(48, 20, 13, 1),(49, 20, 10, 1),(50, 20, 14, 4),
(51, 21, 6, 3),(52, 21, 20, 1),(53, 21, 13, 1),(54, 21, 10, 1),(55, 21, 19, 1),
(56, 21, 21, 1),(57, 21, 14, 1),(58, 21, 15, 1),(59, 22, 19, 1),(60, 22, 14, 1),
(61, 23, 9, 1),(62, 23, 10, 1),(63, 23, 13, 1),(64, 24, 10, 1),(65, 24, 6, 1),
(66, 25, 20, 1),(67, 25, 9, 1),(68, 25, 21, 1),(69, 25, 22, 1),(70, 26, 13, 1),
(71, 26, 14, 2),(72, 26, 15, 2),(73, 27, 14, 1),(74, 27, 19, 1),(75, 28, 10, 1),
(76, 28, 9, 1),(77, 29, 6, 2),(78, 29, 13, 1),(79, 29, 10, 1),(80, 30, 13, 1),
(81, 30, 14, 1),(82, 30, 20, 1),(83, 30, 9, 2),(84, 30, 10, 1),(85, 30, 6, 2),
(86, 31, 10, 3),(87, 32, 14, 2),(88, 32, 20, 2),(89, 32, 13, 1),(90, 32, 19, 1),
(91, 32, 5, 1),(92, 32, 9, 3),(93, 33, 13, 1),(94, 33, 9, 3),(95, 34, 13, 3),
(96, 34, 14, 1),(97, 34, 5, 1),(98, 35, 9, 1),(99, 35, 20, 1),(100, 35, 19, 1),
(101, 35, 14, 1),(102, 35, 4, 1),(103, 35, 21, 1),(104, 36, 14, 1),(105, 36, 6, 1),
(106, 36, 10, 1),(107, 37, 13, 1),(108, 37, 4, 1),(109, 37, 20, 1),(110, 38, 13, 1),
(111, 38, 14, 1),(112, 39, 13, 1),(113, 39, 19, 1),(114, 39, 20, 1),(115, 40, 4, 1),
(116, 40, 9, 1),(117, 40, 5, 1),(118, 40, 6, 1),(119, 41, 6, 1),(120, 41, 9, 4),
(121, 42, 9, 1),(122, 42, 14, 1),(123, 42, 13, 2),(124, 42, 20, 2),(125, 42, 19, 1),
(126, 43, 10, 1),(127, 43, 9, 1),(128, 43, 15, 1),(129, 43, 14, 1),(130, 43, 19, 1),
(131, 44, 10, 1),(132, 44, 13, 1),(133, 45, 9, 1),(134, 45, 13, 2),(135, 45, 4, 1),
(136, 46, 10, 4),(137, 47, 6, 1),(138, 47, 19, 1),(139, 47, 4, 1),(140, 47, 5, 1),
(141, 48, 13, 2),(142, 48, 14, 1),(143, 49, 6, 1),(144, 49, 5, 1),(145, 50, 5, 1),
(146, 50, 9, 3),(147, 50, 28, 1),(148, 50, 14, 4),(149, 50, 19, 1),(150, 54, 9, 1),
(151, 54, 14, 1),(152, 55, 14, 1),(153, 55, 9, 1),(154, 56, 10, 1),(155, 56, 14, 1),
(156, 57, 10, 1),(157, 57, 14, 1),(158, 58, 13, 1),(159, 58, 10, 1),(160, 59, 14, 1),
(161, 59, 9, 1),(162, 60, 4, 1),(163, 60, 5, 1),(164, 61, 9, 1),(165, 61, 14, 1),
(166, 62, 6, 1),(167, 62, 20, 1),(168, 63, 20, 4),(169, 64, 9, 1),(170, 64, 13, 1),
(171, 65, 9, 1),(172, 65, 15, 1),(173, 70, 9, 1),(174, 70, 10, 1),(175, 72, 10, 1),
(176, 72, 9, 1),(177, 73, 6, 1),(178, 76, 10, 1),(179, 76, 30, 4),(180, 77, 9, 1),
(181, 78, 10, 1),(182, 78, 14, 1),(183, 81, 9, 1),(184, 82, 15, 1),(185, 82, 19, 1),
(186, 83, 10, 1),(187, 84, 13, 1),(188, 84, 14, 1),(189, 85, 9, 1),(190, 85, 6, 1),
(191, 86, 20, 2),(192, 86, 15, 1),(193, 86, 5, 2),(194, 86, 23, 1),(195, 86, 6, 1),
(196, 87, 6, 1),(197, 88, 5, 1),(198, 88, 9, 1),(199, 89, 6, 1),(200, 90, 6, 1),
(201, 91, 5, 1),(202, 94, 9, 1),(203, 95, 9, 1);

-- ============================================================
-- DATA: product_attribute
-- ============================================================
INSERT INTO `product_attribute` (`id`, `product_id`, `attribute_id`, `value_text`, `value_decimal`, `value_int`, `attribute_value_id`) VALUES
(117, 8, 10, NULL, NULL, NULL, NULL),(118, 10, 10, NULL, NULL, NULL, NULL),
(119, 11, 10, NULL, NULL, NULL, NULL),(120, 15, 10, NULL, NULL, NULL, NULL),
(121, 17, 10, NULL, NULL, NULL, NULL),(122, 18, 10, NULL, NULL, NULL, NULL),
(123, 19, 10, NULL, NULL, NULL, NULL),(124, 20, 10, NULL, NULL, NULL, NULL),
(125, 21, 10, NULL, NULL, NULL, NULL),(126, 22, 10, NULL, NULL, NULL, NULL),
(127, 23, 10, NULL, NULL, NULL, NULL),(128, 27, 10, NULL, NULL, NULL, NULL),
(129, 28, 10, NULL, NULL, NULL, NULL),
(134, 8, 2, '', 0, 3000, NULL),(135, 10, 2, '', 0, 3000, NULL),
(136, 11, 2, NULL, NULL, NULL, NULL),(137, 15, 2, NULL, NULL, 3100, NULL),
(138, 17, 2, NULL, NULL, NULL, NULL),(139, 18, 2, NULL, NULL, NULL, NULL),
(140, 19, 2, NULL, NULL, NULL, NULL),(141, 20, 2, NULL, NULL, NULL, NULL),
(142, 21, 2, NULL, NULL, NULL, NULL),(143, 22, 2, NULL, NULL, NULL, NULL),
(144, 23, 2, NULL, NULL, NULL, NULL),(145, 27, 2, NULL, NULL, NULL, NULL),
(146, 28, 2, NULL, NULL, NULL, NULL),
(173, 5, 1, NULL, NULL, NULL, NULL),(174, 26, 1, NULL, NULL, NULL, NULL),
(177, 5, 5, NULL, NULL, NULL, NULL),(178, 26, 5, NULL, NULL, NULL, NULL),
(186, 5, 3, NULL, 24, NULL, NULL),(187, 26, 3, NULL, NULL, NULL, NULL),
(191, 37, 2, '', 0, 2100, NULL),
(253, 38, 3, NULL, 2000, NULL, NULL),(254, 39, 3, NULL, NULL, NULL, NULL),
(255, 40, 3, NULL, NULL, NULL, NULL),(256, 41, 3, NULL, NULL, NULL, NULL),
(257, 42, 11, '', NULL, NULL, 33),(258, 43, 11, NULL, NULL, NULL, 33),
(259, 44, 11, NULL, NULL, NULL, 34),(260, 45, 11, NULL, NULL, NULL, 35),
(261, 46, 12, NULL, NULL, NULL, NULL);

-- ============================================================
-- DATA: product_image
-- ============================================================
INSERT INTO `product_image` (`id`, `product_id`, `variant_id`, `url`) VALUES
(323, 5, 13, '41d505a2-bf7b-46ac-b386-ac0c3420de75_asus-vivobook-go-15-e1504fa-r5-nj630w-glr-3-1-180x125.jpg'),
(324, 5, 14, '9fff9e7f-d4d5-4063-ad8e-6d71beacb6e1_asus-vivobook-go-15-e1504fa-r5-nj630w-glr-14-750x500.jpg'),
(325, 5, 23, '46ff5ff9-8821-4778-93e6-59fdd1aed48f_asus-vivobook-go-15-e1504fa-r5-nj630w-glr-3-1-180x125.jpg'),
(326, 10, 6, '4a51b715-b48f-4cb5-8165-d2a0919d9747_samsung-galaxy-s25-ultra-1tb-thumb-600x600.jpg'),
(327, 8, 4, '5cb38ac1-be2d-462c-a438-cf54ab968333_samsung-galaxy-a06-green-thumbn-600x600.jpg'),
(328, 8, 5, '63ad8e7b-4f77-48cb-bfd2-1c597435c9a4_samsung-galaxy-a06-5g-black-thumbn-600x600.jpg'),
(329, 15, 20, '195c3ef7-2a5d-4e6a-91a6-ac62c3d336d2_iphone-17-pro-max-xanh-duong-thumb-600x600.jpg'),
(330, 15, 19, '9030abe6-b63d-444c-a2d7-7101426aebea_download.jpg'),
(331, 26, 9, '352eb0c9-5b12-43b1-9244-47504d98433b_dell-inspiron-15-3530-i7-n5i7216w1-3-638840241491993434-750x500.jpg'),
(332, 26, 10, '44a1032f-af43-410e-ab73-a652faa52254_download.jpg'),
(333, 26, 21, '666fcab0-8be8-4fcf-baed-87f05937e611_dell-inspiron-15-3530-i7-n5i7216w1-11-638840241516606627-750x500.jpg'),
(334, 26, 22, 'bfd1b4de-039f-497e-8765-f002f0de7dd4_dell-inspiron-15-3530-i7-n5i7216w1-thumb-638840241952547194-600x600.jpg'),
(335, 27, 15, '239e3639-c0fc-47ad-9cae-ff892ce8ec3b_xanh_d19c3d1580d34a45a0dfca7ad7499de7_master.jpg'),
(336, 42, 33, '8e45a0eb-e753-4a6e-bff6-4c55e87ba5c6_unnamed.jpg'),
(337, 42, 33, '68c6519c-330f-43bb-9d35-6d6ac8af398c_unnamed3.jpg'),
(338, 42, 33, 'cc522e55-1f1f-464b-9766-6b067cf9d3bf_unnamed2.jpg'),
(339, 43, 34, 'e31a5afb-32e6-4500-a537-db764c28e358_unnamed4.jpg'),
(340, 43, 34, 'f0e967f8-56df-43ea-8f44-be18a64d83d7_unnamed5.jpg'),
(341, 44, 35, '5987c4f8-8d60-49de-b36a-10e5b52ef7bd_unnamed12.jpg'),
(342, 44, 35, 'fcb62d54-45dc-4461-982a-d473eee502f9_a4.jpg'),
(343, 44, 35, 'f7523175-badb-4b68-86d5-9d935bf64211_qa4.jpg'),
(344, 44, 35, 'dd9f56bb-1279-4574-b74b-0cfbfa9a1861_a3.jpg'),
(345, 44, 35, '434a3988-e82c-4371-8c60-d6bfe42b06c5_a2.jpg'),
(346, 45, 37, '60d15d4a-03c8-48c4-b555-1c4bbf1d0c29_a5.jpg'),
(347, 45, 37, '3a217044-f33b-4b94-9a07-cfc3a0fea13a_a6.jpg'),
(348, 41, 31, 'b9f190bf-969c-4388-be33-3f9a6de033b6_msi-13.jpg'),
(349, 41, 32, 'd238a3df-d609-4dc1-9b46-d2f8529663d6_msi-13.jpg'),
(350, 40, 30, 'd8193786-3b69-4db2-8b23-250dd95244f6_a7.jpg'),
(351, 39, 28, '2072cc79-dff0-4857-9bc6-75fb0ccfd7ad_unnamed..jpg'),
(352, 39, 29, '33e8ed9a-0151-44e4-97d4-73a4ed08079b_unnamed123.jpg'),
(353, 38, 26, '076c9fbf-93d3-420a-bb07-764f754a3be1_acer-aspire-lite-15-51m-5542-i5-nxks5sv001-glr-2-750x500.jpg'),
(354, 38, 27, 'ce2f22a3-8a86-4f8e-a00d-6678a22960ed_acer-aspire-lite-15-51m-55nb-i5-nxkrssv001-3-750x500.jpg'),
(355, 46, 38, '2fd19139-28ff-4d4c-9589-fb383361b3f2_a8.jpg');

-- ============================================================
-- DATA: variant_attribute
-- ============================================================
INSERT INTO `variant_attribute` (`id`, `attribute_id`, `variant_id`, `value_int`, `value_decimal`, `value_text`, `attribute_value_id`) VALUES
(17, 5, 9, 16, NULL, NULL, 15),(19, 5, 10, 16, NULL, NULL, 14),
(25, 5, 13, 17, NULL, NULL, 17),(27, 5, 14, 17, NULL, NULL, 17),
(60, 8, 9, 512, NULL, NULL, 23),(61, 8, 10, 23, NULL, NULL, 23),
(62, 8, 13, 516, NULL, NULL, 23),(63, 8, 14, 23, NULL, NULL, 23),
(64, 6, 9, NULL, NULL, 'Intel Core i5 - 1334U', 1),
(65, 6, 10, NULL, NULL, 'Intel Core i5 - 1235U', 1),
(66, 6, 13, NULL, NULL, '1', 1),(67, 6, 14, NULL, NULL, '2', 2),
(68, 5, 21, 16, NULL, NULL, 16),(69, 8, 21, 22, NULL, NULL, 22),
(70, 6, 21, NULL, NULL, '2', 2),(71, 5, 22, 16, NULL, NULL, 15),
(72, 8, 22, 31, NULL, NULL, 31),
(73, 6, 22, NULL, NULL, 'Intel Core i7 - 1355U', 2),
(74, 5, 23, 15, NULL, NULL, 15),(75, 8, 23, 22, NULL, NULL, 22),
(76, 6, 23, NULL, NULL, '1', 1),(77, 5, 4, NULL, NULL, NULL, 15),
(78, 5, 5, 14, NULL, NULL, 14),(79, 5, 6, NULL, NULL, NULL, 16),
(80, 5, 15, NULL, NULL, NULL, 16),(81, 5, 19, NULL, NULL, NULL, NULL),
(82, 5, 20, NULL, NULL, NULL, NULL),(83, 5, 26, NULL, NULL, NULL, 15),
(84, 8, 26, NULL, NULL, NULL, 23),(85, 6, 26, NULL, NULL, NULL, 1),
(86, 5, 27, 18, NULL, NULL, 18),(87, 8, 27, 23, NULL, NULL, 23),
(88, 6, 27, NULL, NULL, '2', 2),(89, 5, 28, NULL, NULL, NULL, 15),
(90, 8, 28, NULL, NULL, NULL, NULL),(91, 6, 28, NULL, NULL, NULL, 4),
(92, 5, 29, 15, NULL, NULL, 15),(93, 8, 29, NULL, NULL, NULL, 23),
(94, 6, 29, NULL, NULL, NULL, 4),(95, 5, 30, NULL, NULL, NULL, 15),
(96, 8, 30, NULL, NULL, NULL, 23),(97, 6, 30, NULL, NULL, NULL, 5),
(98, 5, 31, NULL, NULL, NULL, 15),(99, 8, 31, NULL, NULL, NULL, 31),
(100, 6, 31, NULL, NULL, NULL, 32),(101, 5, 32, 16, NULL, NULL, 16),
(102, 8, 32, 31, NULL, NULL, 31),(103, 6, 32, NULL, NULL, '32', 32),
(104, 5, 38, NULL, NULL, NULL, 14),(105, 8, 38, NULL, NULL, NULL, 23),
(106, 6, 38, NULL, NULL, NULL, 32);

-- ============================================================
-- DATA: cart
-- ============================================================
INSERT INTO `cart` (`id`, `user_id`, `variant_id`, `quantity`, `unit_price`) VALUES
(292, 40, 9, 2, 12000000),(293, 1, 5, 6, 27000000),(297, 3, 14, 2, 21000000),
(298, 3, 10, 2, 19000000),(312, 1, 9, 4, 12000000),(313, 3, 27, 2, 11999000),
(318, 1, 13, 1, 19000000),(319, 1, 33, 2, 3650000),(320, 1, 30, 1, 16490000),
(321, 2, 15, 3, 29000000),(322, 2, 23, 3, 11990000),(323, 2, 27, 1, 11999000),
(324, 2, 9, 3, 12000000),(325, 2, 26, 2, 12199000),(326, 2, 13, 3, 19000000),
(327, 2, 29, 1, 20590000),(328, 2, 10, 5, 19000000),(329, 2, 30, 1, 16490000),
(330, 2, 30, 1, 16490000),(331, 2, 14, 2, 21000000),(332, 2, 22, 1, 21990000),
(333, 2, 6, 8, 27500000),(334, 1, 27, 3, 11999000),(335, 2, 20, 5, 41000000),
(336, 2, 5, 2, 27000000),(337, 11, 9, 2, 12000000);
