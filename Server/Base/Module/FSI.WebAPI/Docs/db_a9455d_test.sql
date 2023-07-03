-- phpMyAdmin SQL Dump
-- version 5.1.3
-- https://www.phpmyadmin.net/
--
-- Máy chủ: MYSQL5045.site4now.net
-- Thời gian đã tạo: Th3 24, 2023 lúc 03:04 AM
-- Phiên bản máy phục vụ: 8.0.31
-- Phiên bản PHP: 7.4.30

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Cơ sở dữ liệu: `db_a9455d_test`
--

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `accounts`
--

CREATE TABLE `accounts` (
  `Id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `Email` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `PhoneNumber` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `PasswordHash` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ExtraProperties` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `ConcurrencyStamp` varchar(40) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `CreationTime` datetime(6) NOT NULL,
  `CreatorId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `LastModificationTime` datetime(6) DEFAULT NULL,
  `LastModifierId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `IsDeleted` tinyint(1) NOT NULL DEFAULT '0',
  `DeleterId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `DeletionTime` datetime(6) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Đang đổ dữ liệu cho bảng `accounts`
--

INSERT INTO `accounts` (`Id`, `Email`, `PhoneNumber`, `PasswordHash`, `ExtraProperties`, `ConcurrencyStamp`, `CreationTime`, `CreatorId`, `LastModificationTime`, `LastModifierId`, `IsDeleted`, `DeleterId`, `DeletionTime`) VALUES
('3a0a2410-a95b-507b-9da9-46e219e88998', 'vhieukk20000@gmail.com', '0971883025', '$2a$11$P5F3ST7fZL4AWEASzd6dqu6QMEYMjBY4U1qdFNCG/ZtBlEcVk1SMK', '{}', '637d844bd5234afe99b7e3c4ff310674', '2023-03-24 11:32:42.201193', NULL, NULL, NULL, 0, NULL, NULL),
('3a0a24df-bd6b-5360-0eda-0af0c4adee21', 'string@gmail.com', '0987654321', '$2a$11$sPmWtUvd.ulBLuzQ/QIEVOO5spK1n.eZC9c868cNk4C08kvJhF7BG', '{}', '746063a79ff94f15b71a3ded18942fd6', '2023-03-24 15:18:49.221600', '3a0a2410-d832-aced-882c-4bdcb0da2805', NULL, NULL, 0, NULL, NULL);

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `conversations`
--

CREATE TABLE `conversations` (
  `Id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `JustTwoPeople` tinyint(1) NOT NULL,
  `UserAId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `UserBId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `ConversationName` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ConversationAvatar` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Tag` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `JoinLink` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `LastMessageId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `ExtraProperties` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `ConcurrencyStamp` varchar(40) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `CreationTime` datetime(6) NOT NULL,
  `CreatorId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `LastModificationTime` datetime(6) DEFAULT NULL,
  `LastModifierId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `IsDeleted` tinyint(1) NOT NULL DEFAULT '0',
  `DeleterId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `DeletionTime` datetime(6) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `files`
--

CREATE TABLE `files` (
  `Id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `AuthorId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `Url` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Size` int NOT NULL,
  `ExtraProperties` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `ConcurrencyStamp` varchar(40) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `CreationTime` datetime(6) NOT NULL,
  `CreatorId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `LastModificationTime` datetime(6) DEFAULT NULL,
  `LastModifierId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `IsDeleted` tinyint(1) NOT NULL DEFAULT '0',
  `DeleterId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `DeletionTime` datetime(6) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `fsi_test`
--

CREATE TABLE `fsi_test` (
  `Id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `NAME` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `CODE` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `DESCRIPTION` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ExtraProperties` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `ConcurrencyStamp` varchar(40) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `CreationTime` datetime(6) NOT NULL,
  `CreatorId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `LastModificationTime` datetime(6) DEFAULT NULL,
  `LastModifierId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `IsDeleted` tinyint(1) NOT NULL DEFAULT '0',
  `DeleterId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `DeletionTime` datetime(6) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Đang đổ dữ liệu cho bảng `fsi_test`
--

INSERT INTO `fsi_test` (`Id`, `NAME`, `CODE`, `DESCRIPTION`, `ExtraProperties`, `ConcurrencyStamp`, `CreationTime`, `CreatorId`, `LastModificationTime`, `LastModifierId`, `IsDeleted`, `DeleterId`, `DeletionTime`) VALUES
('3a0a24fd-66c8-353b-d52b-faf7c55a4f83', 'hieunv', '123', 'helo', '{}', '1eb4222af2b2400c83e352d139c4d2fb', '2023-03-24 15:51:12.268829', '3a0a2410-d832-aced-882c-4bdcb0da2805', NULL, NULL, 0, NULL, NULL);

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `messages`
--

CREATE TABLE `messages` (
  `Id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `ConversationId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `SenderId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `Index` int NOT NULL,
  `Type` int NOT NULL,
  `Content` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `FocusToMessageId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `ExtraProperties` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `ConcurrencyStamp` varchar(40) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `CreationTime` datetime(6) NOT NULL,
  `CreatorId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `LastModificationTime` datetime(6) DEFAULT NULL,
  `LastModifierId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `IsDeleted` tinyint(1) NOT NULL DEFAULT '0',
  `DeleterId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `DeletionTime` datetime(6) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `userconnections`
--

CREATE TABLE `userconnections` (
  `Id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `UserId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `ConnectionId` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ExtraProperties` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `ConcurrencyStamp` varchar(40) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `CreationTime` datetime(6) NOT NULL,
  `CreatorId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `LastModificationTime` datetime(6) DEFAULT NULL,
  `LastModifierId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `IsDeleted` tinyint(1) NOT NULL DEFAULT '0',
  `DeleterId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `DeletionTime` datetime(6) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Đang đổ dữ liệu cho bảng `userconnections`
--

INSERT INTO `userconnections` (`Id`, `UserId`, `ConnectionId`, `ExtraProperties`, `ConcurrencyStamp`, `CreationTime`, `CreatorId`, `LastModificationTime`, `LastModifierId`, `IsDeleted`, `DeleterId`, `DeletionTime`) VALUES
('3a0a2506-2d4e-c193-b6fe-dd1adcd2aac8', '3a0a2410-d832-aced-882c-4bdcb0da2805', 'Yu5zTmFMvDyUCSxoUKXh4Q', '{}', 'e725232f93cd404ab83e2a72fb223731', '2023-03-24 16:00:45.775160', '3a0a2410-d832-aced-882c-4bdcb0da2805', NULL, NULL, 0, NULL, NULL);

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `userconversations`
--

CREATE TABLE `userconversations` (
  `Id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `ConversationId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `UserId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `RoleInConversation` int NOT NULL,
  `NickName` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `LastIndexSeen` int NOT NULL,
  `IsActive` tinyint(1) NOT NULL,
  `EnableNotification` tinyint(1) NOT NULL,
  `OffNotificationTo` datetime(6) NOT NULL,
  `IsStorage` tinyint(1) NOT NULL,
  `ExtraProperties` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `ConcurrencyStamp` varchar(40) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `CreationTime` datetime(6) NOT NULL,
  `CreatorId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `LastModificationTime` datetime(6) DEFAULT NULL,
  `LastModifierId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `IsDeleted` tinyint(1) NOT NULL DEFAULT '0',
  `DeleterId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `DeletionTime` datetime(6) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `userroots`
--

CREATE TABLE `userroots` (
  `Id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `Name` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Phone` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `DateOfBirth` datetime(6) NOT NULL,
  `IdentityCard` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Location` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `WorkingPlace` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `AccountId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `Discriminator` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Speciality` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `Personality` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `Skill` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `WorkingExperience` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `Activity` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `Certificate` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `Award` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `FavoriteField` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `hasProject` tinyint(1) DEFAULT NULL,
  `ExtraProperties` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `ConcurrencyStamp` varchar(40) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `CreationTime` datetime(6) NOT NULL,
  `CreatorId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `LastModificationTime` datetime(6) DEFAULT NULL,
  `LastModifierId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `IsDeleted` tinyint(1) NOT NULL DEFAULT '0',
  `DeleterId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `DeletionTime` datetime(6) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Đang đổ dữ liệu cho bảng `userroots`
--

INSERT INTO `userroots` (`Id`, `Name`, `Phone`, `DateOfBirth`, `IdentityCard`, `Location`, `WorkingPlace`, `AccountId`, `Discriminator`, `Speciality`, `Personality`, `Skill`, `WorkingExperience`, `Activity`, `Certificate`, `Award`, `FavoriteField`, `hasProject`, `ExtraProperties`, `ConcurrencyStamp`, `CreationTime`, `CreatorId`, `LastModificationTime`, `LastModifierId`, `IsDeleted`, `DeleterId`, `DeletionTime`) VALUES
('3a0a2410-d832-aced-882c-4bdcb0da2805', 'Nguyễn Văn Hiếu', '0971883025', '2023-03-24 04:27:41.323000', '1800000000', 'Hà Tĩnh', 'Hà Nội', '3a0a2410-a95b-507b-9da9-46e219e88998', 'Founder', 'string', 'string', 'string', 'string', 'string', 'string', 'string', 'string', 0, '{}', 'da7b4264604043328bd6d6e9e4221ff6', '2023-03-24 11:32:47.811136', NULL, '2023-03-24 11:55:20.457562', '3a0a2410-d832-aced-882c-4bdcb0da2805', 0, NULL, NULL),
('3a0a24df-c716-e1c6-06c8-bfd0cdde0a6d', 'string', 'string', '2023-03-24 08:18:05.597000', 'string', 'string', 'string', '3a0a24df-bd6b-5360-0eda-0af0c4adee21', 'Founder', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, '{}', '1ac55fda05734b2881ed0aab87262c81', '2023-03-24 15:18:49.297946', '3a0a2410-d832-aced-882c-4bdcb0da2805', NULL, NULL, 0, NULL, NULL);

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `__efmigrationshistory`
--

CREATE TABLE `__efmigrationshistory` (
  `MigrationId` varchar(150) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ProductVersion` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Đang đổ dữ liệu cho bảng `__efmigrationshistory`
--

INSERT INTO `__efmigrationshistory` (`MigrationId`, `ProductVersion`) VALUES
('20230324035721_reset', '6.0.8');

--
-- Chỉ mục cho các bảng đã đổ
--

--
-- Chỉ mục cho bảng `accounts`
--
ALTER TABLE `accounts`
  ADD PRIMARY KEY (`Id`),
  ADD UNIQUE KEY `IX_Accounts_Email` (`Email`),
  ADD UNIQUE KEY `IX_Accounts_PhoneNumber` (`PhoneNumber`);

--
-- Chỉ mục cho bảng `conversations`
--
ALTER TABLE `conversations`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `IX_Conversations_UserAId` (`UserAId`),
  ADD KEY `IX_Conversations_UserBId` (`UserBId`);

--
-- Chỉ mục cho bảng `files`
--
ALTER TABLE `files`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `IX_Files_AuthorId` (`AuthorId`);

--
-- Chỉ mục cho bảng `fsi_test`
--
ALTER TABLE `fsi_test`
  ADD PRIMARY KEY (`Id`);

--
-- Chỉ mục cho bảng `messages`
--
ALTER TABLE `messages`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `IX_Messages_ConversationId` (`ConversationId`),
  ADD KEY `IX_Messages_SenderId` (`SenderId`);

--
-- Chỉ mục cho bảng `userconnections`
--
ALTER TABLE `userconnections`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `IX_UserConnections_UserId` (`UserId`);

--
-- Chỉ mục cho bảng `userconversations`
--
ALTER TABLE `userconversations`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `IX_UserConversations_Conversa~` (`ConversationId`),
  ADD KEY `IX_UserConversations_UserId` (`UserId`);

--
-- Chỉ mục cho bảng `userroots`
--
ALTER TABLE `userroots`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `IX_UserRoots_AccountId` (`AccountId`);

--
-- Chỉ mục cho bảng `__efmigrationshistory`
--
ALTER TABLE `__efmigrationshistory`
  ADD PRIMARY KEY (`MigrationId`);

--
-- Các ràng buộc cho các bảng đã đổ
--

--
-- Các ràng buộc cho bảng `conversations`
--
ALTER TABLE `conversations`
  ADD CONSTRAINT `FK_Conversations_UserRoots_Us~` FOREIGN KEY (`UserAId`) REFERENCES `userroots` (`Id`),
  ADD CONSTRAINT `FK_Conversations_UserRoots_U~1` FOREIGN KEY (`UserBId`) REFERENCES `userroots` (`Id`);

--
-- Các ràng buộc cho bảng `files`
--
ALTER TABLE `files`
  ADD CONSTRAINT `FK_Files_UserRoots_AuthorId` FOREIGN KEY (`AuthorId`) REFERENCES `userroots` (`Id`) ON DELETE CASCADE;

--
-- Các ràng buộc cho bảng `messages`
--
ALTER TABLE `messages`
  ADD CONSTRAINT `FK_Messages_Conversations_Con~` FOREIGN KEY (`ConversationId`) REFERENCES `conversations` (`Id`) ON DELETE CASCADE,
  ADD CONSTRAINT `FK_Messages_UserRoots_SenderId` FOREIGN KEY (`SenderId`) REFERENCES `userroots` (`Id`) ON DELETE CASCADE;

--
-- Các ràng buộc cho bảng `userconnections`
--
ALTER TABLE `userconnections`
  ADD CONSTRAINT `FK_UserConnections_UserRoots_~` FOREIGN KEY (`UserId`) REFERENCES `userroots` (`Id`) ON DELETE CASCADE;

--
-- Các ràng buộc cho bảng `userconversations`
--
ALTER TABLE `userconversations`
  ADD CONSTRAINT `FK_UserConversations_Conversa~` FOREIGN KEY (`ConversationId`) REFERENCES `conversations` (`Id`) ON DELETE CASCADE,
  ADD CONSTRAINT `FK_UserConversations_UserRoot~` FOREIGN KEY (`UserId`) REFERENCES `userroots` (`Id`) ON DELETE CASCADE;

--
-- Các ràng buộc cho bảng `userroots`
--
ALTER TABLE `userroots`
  ADD CONSTRAINT `FK_UserRoots_Accounts_Account~` FOREIGN KEY (`AccountId`) REFERENCES `accounts` (`Id`) ON DELETE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
