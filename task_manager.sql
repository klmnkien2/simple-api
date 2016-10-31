-- phpMyAdmin SQL Dump
-- version 4.5.1
-- http://www.phpmyadmin.net
--
-- Host: 127.0.0.1
-- Generation Time: Oct 31, 2016 at 05:14 PM
-- Server version: 10.1.9-MariaDB
-- PHP Version: 5.6.15

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `task_manager`
--

-- --------------------------------------------------------

--
-- Table structure for table `friends`
--

CREATE TABLE `friends` (
  `user1` int(11) NOT NULL,
  `user2` int(11) NOT NULL,
  `request_time` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `type` tinyint(2) NOT NULL DEFAULT '0' COMMENT '-1=ignore,0=pend,1=friend',
  `ignore_type` tinyint(2) NOT NULL DEFAULT '1' COMMENT 'who want ignore? 1=user1,2=user2,3=both'
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `friends`
--

INSERT INTO `friends` (`user1`, `user2`, `request_time`, `type`, `ignore_type`) VALUES
(10, 12, '2016-07-19 03:28:55', 0, 1),
(12, 1000015991, '2016-01-20 17:57:28', -1, 2),
(13, 1000015991, '2016-01-11 14:03:21', 0, 1),
(16, 1000015991, '2016-01-20 17:56:29', 0, 0),
(17, 1000015991, '2016-01-20 18:05:18', -1, 2),
(18, 1000015991, '2016-01-20 18:00:22', -1, 3),
(1000015991, 10, '2016-01-11 13:59:54', 1, 1),
(1000015991, 11, '2016-01-11 14:03:21', -1, 1),
(1000015991, 14, '2016-01-20 17:58:42', 1, 0),
(1000015991, 15, '2016-02-24 11:43:53', -1, 1);

-- --------------------------------------------------------

--
-- Table structure for table `messages`
--

CREATE TABLE `messages` (
  `message_id` int(11) NOT NULL,
  `user_id` int(11) NOT NULL,
  `user_name` varchar(255) NOT NULL,
  `receive_id` int(11) DEFAULT NULL,
  `receive_name` varchar(255) DEFAULT NULL,
  `room_id` int(11) NOT NULL,
  `create_time` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `message` text NOT NULL,
  `notify` tinyint(1) NOT NULL DEFAULT '0',
  `is_read` tinyint(1) NOT NULL DEFAULT '0'
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `messages`
--

INSERT INTO `messages` (`message_id`, `user_id`, `user_name`, `receive_id`, `receive_name`, `room_id`, `create_time`, `message`, `notify`, `is_read`) VALUES
(20, 1000015991, 'admintest1', 0, '', 2, '2016-09-02 03:25:22', 'test', 0, 1),
(21, 1000015991, 'admintest1', 0, '', 2, '2016-09-02 03:25:31', 'oai sao lai bi duplicate', 0, 1),
(22, 1000015991, 'admintest1', 0, '', 2, '2016-09-02 03:31:22', '=))', 0, 1),
(23, 1000015991, 'admintest1', 0, '', 2, '2016-09-02 03:31:38', 'test xem nao', 0, 1),
(24, 1000015991, 'admintest1', 0, '', 2, '2016-09-02 03:42:38', 'chat thu xem nao', 0, 1),
(25, 1000015991, 'admintest1', 0, '', 2, '2016-09-02 03:42:43', 'ok tat ca deu on', 0, 1),
(26, 1000015991, 'admintest1', 10, 'tranvutuan', 0, '2016-09-04 07:53:02', 'hay nhi', 0, 1),
(27, 1000015991, 'admintest1', 10, 'tranvutuan', 0, '2016-09-04 08:00:55', 'test', 0, 0),
(28, 1000015991, 'admintest1', 10, 'tranvutuan', 0, '2016-09-04 08:01:06', 'sao lai ko len song', 0, 0),
(29, 1000015991, 'admintest1', 10, 'tranvutuan', 0, '2016-09-04 08:01:19', 'cai meo gi the nay', 0, 1),
(30, 1000015991, 'admintest1', 13, 'dinhtan89', 0, '2016-09-04 08:01:47', 'o hay', 0, 1),
(31, 1000015991, 'admintest1', 13, 'dinhtan89', 0, '2016-09-04 08:02:01', 'quai la nhi', 0, 1),
(32, 1000015991, 'admintest1', 13, 'dinhtan89', 0, '2016-09-04 08:02:11', 'o la la', 0, 1);

-- --------------------------------------------------------

--
-- Table structure for table `rooms`
--

CREATE TABLE `rooms` (
  `room_id` int(11) NOT NULL,
  `room_name` varchar(255) CHARACTER SET utf8 NOT NULL,
  `parent_id` int(11) NOT NULL,
  `has_child` tinyint(4) NOT NULL,
  `server_id` int(11) NOT NULL,
  `members` smallint(6) NOT NULL COMMENT 'current of mem',
  `maximum` smallint(6) NOT NULL COMMENT 'max of member',
  `create_time` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `image` varchar(1000) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `rooms`
--

INSERT INTO `rooms` (`room_id`, `room_name`, `parent_id`, `has_child`, `server_id`, `members`, `maximum`, `create_time`, `image`) VALUES
(1, 'GameTV', 0, 1, -1, 0, 100, '2016-10-31 12:56:47', ''),
(2, 'GameTV Room 1', 1, 0, 1, 0, 100, '2016-10-31 12:56:55', ''),
(3, 'GameTV Room 2', 1, 0, 2, 0, 100, '2016-10-31 12:56:55', ''),
(4, 'GameTV Pro 1', 1, 0, 3, 0, 100, '2016-10-31 12:56:55', ''),
(5, 'GameTV Pro 2', 1, 0, 4, 0, 100, '2016-10-31 15:54:11', ''),
(6, 'Clan Hà Nội', 0, 1, -1, 0, 100, '2016-10-31 12:56:47', ''),
(7, 'Clan Thái Bình', 0, 1, -1, 0, 100, '2016-10-31 12:56:47', ''),
(8, 'Bibi Club', 0, 1, -1, 0, 100, '2016-10-31 12:56:47', ''),
(9, 'VEC 1', 6, 0, 5, 0, 100, '2016-10-31 13:07:40', ''),
(10, 'VEC 2', 6, 0, 6, 1, 100, '2016-10-31 13:07:40', ''),
(11, 'AOE17 1', 7, 0, 7, 0, 100, '2016-10-31 13:07:41', ''),
(12, 'AOE17 2', 7, 0, 8, 0, 100, '2016-10-31 13:07:41', ''),
(13, 'Bibi 1', 8, 0, 9, 0, 100, '2016-10-31 13:07:42', '');

-- --------------------------------------------------------

--
-- Table structure for table `room_users`
--

CREATE TABLE `room_users` (
  `room_id` int(11) NOT NULL,
  `user_id` int(11) NOT NULL,
  `user_name` varchar(255) CHARACTER SET utf8 NOT NULL,
  `ip` varchar(255) CHARACTER SET utf8 NOT NULL,
  `join_time` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `last_active` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `ping` varchar(10) CHARACTER SET utf8 DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `room_users`
--

INSERT INTO `room_users` (`room_id`, `user_id`, `user_name`, `ip`, `join_time`, `last_active`, `ping`) VALUES
(10, 1000015991, 'admintest1', '192.168.40.110', '2016-10-31 16:09:05', '2016-10-31 16:09:29', ' 7ms\r\n');

-- --------------------------------------------------------

--
-- Table structure for table `servers`
--

CREATE TABLE `servers` (
  `server_id` int(11) NOT NULL,
  `host` varchar(50) CHARACTER SET utf8 NOT NULL,
  `port` int(11) NOT NULL,
  `hub` varchar(255) CHARACTER SET utf8 NOT NULL,
  `create_time` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `number_connected` int(11) NOT NULL DEFAULT '0'
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `servers`
--

INSERT INTO `servers` (`server_id`, `host`, `port`, `hub`, `create_time`, `number_connected`) VALUES
(1, '103.56.157.252', 5561, 'aoe_vpn_hub10', '2016-01-25 17:16:11', 0),
(2, '103.56.157.252', 992, 'aoe_vpn_hub', '2016-01-25 17:14:46', 0),
(3, '103.56.157.252', 5556, 'aoe_vpn_hub2', '2016-01-25 17:14:46', 0),
(4, '103.56.157.252', 443, 'aoe_vpn_hub3', '2016-01-25 17:16:11', 0),
(5, '103.56.157.252', 5555, 'aoe_vpn_hub4', '2016-01-25 17:16:11', 0),
(6, '103.56.157.252', 5556, 'aoe_vpn_hub5', '2016-01-25 17:16:11', 0),
(7, '103.56.157.252', 5557, 'aoe_vpn_hub6', '2016-01-25 17:16:11', 0),
(8, '103.56.157.252', 5558, 'aoe_vpn_hub7', '2016-01-25 17:16:11', 0),
(9, '103.56.157.252', 5559, 'aoe_vpn_hub8', '2016-01-25 17:16:11', 0),
(10, '103.56.157.252', 5560, 'aoe_vpn_hub9', '2016-01-25 17:16:11', 0);

-- --------------------------------------------------------

--
-- Table structure for table `user_caches`
--

CREATE TABLE `user_caches` (
  `user_id` int(11) NOT NULL,
  `user_name` varchar(100) NOT NULL,
  `user_email` varchar(255) NOT NULL,
  `password` varchar(100) NOT NULL,
  `last_active` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `avatar` varchar(1000) NOT NULL,
  `level` smallint(6) NOT NULL,
  `diamond` int(11) NOT NULL,
  `status` varchar(1000) NOT NULL,
  `state` tinyint(2) NOT NULL DEFAULT '1' COMMENT '-1=off,0=invi,1=onl'
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `user_caches`
--

INSERT INTO `user_caches` (`user_id`, `user_name`, `user_email`, `password`, `last_active`, `avatar`, `level`, `diamond`, `status`, `state`) VALUES
(10, 'tranvutuan', '', 'Tuan@123', '2016-08-18 15:37:19', '', 0, 0, 'yeu doi vai luong', 0),
(11, 'kimhc0210', '', 'A123987@@', '2016-08-01 10:26:57', '', 0, 0, '', 1),
(12, 'CyberZ', '', 'Zone@$132', '2016-08-01 10:26:31', '', 0, 0, '', 1),
(13, 'dinhtan89', '', 'Dinhtan89', '2016-08-01 16:05:14', '', 0, 0, '', 1),
(14, 'qthai2502', '', 'ABC123456789', '2016-08-01 15:58:50', '', 0, 0, '', 1),
(15, 'duongdt49mt221189', '', 'D07u4ndu0n6', '2016-04-13 10:14:09', '', 0, 0, '', 1),
(16, 'wantlose', '', 'Duong168', '2016-08-01 10:26:39', '', 0, 0, '', 1),
(17, 'duongdt', '', 'D07u4ndu0n6', '2016-08-01 10:28:37', '', 0, 0, '', 1),
(18, 'kakashilfc', '', 'Kakashi123@', '2016-08-01 10:27:41', '', 0, 0, '', 1),
(19, 'angocbkhn', '', 'Ngoc12a3', '2016-08-01 10:27:14', '', 0, 0, '', 1),
(20, 'batvuonggia', '', 'Loveofromeo@@123', '2016-07-22 07:33:55', '', 0, 0, '', 1),
(21, 'nguycongtu', '', 'Vxn6bbx5', '2016-08-01 10:10:13', '', 0, 0, '', 1),
(1000015991, 'admintest1', '', 'Admin12345678', '2016-10-31 16:11:27', 'http://trading.gametv.vn/public_html/themes/default/img/user/noavatar.jpg', 0, 0, 'toi la toi', 1);

--
-- Indexes for dumped tables
--

--
-- Indexes for table `friends`
--
ALTER TABLE `friends`
  ADD PRIMARY KEY (`user1`,`user2`);

--
-- Indexes for table `messages`
--
ALTER TABLE `messages`
  ADD PRIMARY KEY (`message_id`);

--
-- Indexes for table `rooms`
--
ALTER TABLE `rooms`
  ADD PRIMARY KEY (`room_id`);

--
-- Indexes for table `room_users`
--
ALTER TABLE `room_users`
  ADD PRIMARY KEY (`room_id`,`user_id`),
  ADD UNIQUE KEY `room_id` (`room_id`,`user_id`);

--
-- Indexes for table `servers`
--
ALTER TABLE `servers`
  ADD PRIMARY KEY (`server_id`);

--
-- Indexes for table `user_caches`
--
ALTER TABLE `user_caches`
  ADD PRIMARY KEY (`user_id`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `messages`
--
ALTER TABLE `messages`
  MODIFY `message_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=33;
--
-- AUTO_INCREMENT for table `rooms`
--
ALTER TABLE `rooms`
  MODIFY `room_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=14;
--
-- AUTO_INCREMENT for table `servers`
--
ALTER TABLE `servers`
  MODIFY `server_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=11;
--
-- AUTO_INCREMENT for table `user_caches`
--
ALTER TABLE `user_caches`
  MODIFY `user_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=1000015992;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
