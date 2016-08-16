-- phpMyAdmin SQL Dump
-- version 4.3.11
-- http://www.phpmyadmin.net
--
-- Host: 127.0.0.1
-- Generation Time: Aug 16, 2016 at 10:52 AM
-- Server version: 5.6.24
-- PHP Version: 5.6.8

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;

--
-- Database: `task_manager`
--

-- --------------------------------------------------------

--
-- Table structure for table `friends`
--

CREATE TABLE IF NOT EXISTS `friends` (
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
(9, 10, '2016-01-11 13:59:54', 1, 1),
(9, 11, '2016-01-11 14:03:21', -1, 1),
(9, 14, '2016-01-20 17:58:42', 0, 1),
(9, 15, '2016-02-24 11:43:53', -1, 1),
(10, 12, '2016-07-19 03:28:55', 0, 1),
(12, 9, '2016-01-20 17:57:28', -1, 2),
(13, 9, '2016-01-11 14:03:21', 0, 1),
(16, 9, '2016-01-20 17:56:29', 0, 0),
(17, 9, '2016-01-20 18:05:18', -1, 2),
(18, 9, '2016-01-20 18:00:22', -1, 3);

-- --------------------------------------------------------

--
-- Table structure for table `messages`
--

CREATE TABLE IF NOT EXISTS `messages` (
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
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=latin1;

--
-- Dumping data for table `messages`
--

INSERT INTO `messages` (`message_id`, `user_id`, `user_name`, `receive_id`, `receive_name`, `room_id`, `create_time`, `message`, `notify`, `is_read`) VALUES
(1, 9, 'admintest1', 12, 'CyberZ', 0, '2016-08-08 06:35:18', 'a', 0, 1),
(2, 2, 'admintest1', 9, 'CyberZ', 0, '2016-08-08 07:33:27', 'test', 0, 1),
(3, 9, 'admintest1', 12, 'CyberZ', 0, '2016-08-08 07:40:30', 'toi ko hieu', 0, 1),
(4, 9, 'admintest1', 2, 'admintest1', 0, '2016-08-08 08:34:55', 'hien ra nao', 0, 0),
(5, 9, 'admintest1', 12, 'CyberZ', 0, '2016-08-08 08:59:02', 'alala', 0, 1),
(6, 9, 'admintest1', 12, 'CyberZ', 0, '2016-08-08 08:59:18', 'homnay toi muon,homnay toi muon,homnay toi muon,homnay toi muon,homnay toi muon,homnay toi muonhomnay toi muonhomnay toi muonhomnay toi muonhomnay toi muonhomnay toi muonhomnay toi muonhomnay toi muonhomnay toi muonhomnay toi muonhomnay toi muon', 0, 1),
(7, 9, 'admintest1', 12, 'CyberZ', 0, '2016-08-08 09:43:05', 'afff', 0, 1),
(8, 9, 'admintest1', 12, 'CyberZ', 0, '2016-08-08 09:43:11', 'oh la la', 0, 1);

-- --------------------------------------------------------

--
-- Table structure for table `rooms`
--

CREATE TABLE IF NOT EXISTS `rooms` (
  `room_id` int(11) NOT NULL,
  `room_name` varchar(255) CHARACTER SET utf8 NOT NULL,
  `parent_id` int(11) NOT NULL,
  `server_id` int(11) NOT NULL,
  `members` smallint(6) NOT NULL COMMENT 'current of mem',
  `maximum` smallint(6) NOT NULL COMMENT 'max of member',
  `create_time` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP
) ENGINE=InnoDB AUTO_INCREMENT=122 DEFAULT CHARSET=latin1;

--
-- Dumping data for table `rooms`
--

INSERT INTO `rooms` (`room_id`, `room_name`, `parent_id`, `server_id`, `members`, `maximum`, `create_time`) VALUES
(1, 'AOE', 0, 0, 0, 100, '2016-08-01 08:50:38'),
(2, 'Hà nội', 1, 0, 0, 100, '2016-08-01 14:39:19'),
(116, 'Tp Hồ Chí Minh', 1, 0, 0, 100, '2016-08-01 14:39:19'),
(117, 'Hải phòng', 1, 0, 0, 100, '2016-08-01 14:39:19'),
(118, 'Cầu giấy', 2, 0, 0, 100, '2016-08-01 14:39:19'),
(119, 'Trần Duy Hưng', 2, 0, 0, 100, '2016-08-01 14:39:19'),
(120, 'Quận 1', 116, 0, 0, 100, '2016-08-01 14:39:19'),
(121, 'Quận 3', 116, 0, 0, 100, '2016-08-01 14:39:19');

-- --------------------------------------------------------

--
-- Table structure for table `room_users`
--

CREATE TABLE IF NOT EXISTS `room_users` (
  `room_id` int(11) NOT NULL,
  `user_id` int(11) NOT NULL,
  `user_name` varchar(255) NOT NULL,
  `ip` varchar(255) NOT NULL,
  `join_time` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `last_active` timestamp NOT NULL DEFAULT '0000-00-00 00:00:00'
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `room_users`
--

INSERT INTO `room_users` (`room_id`, `user_id`, `user_name`, `ip`, `join_time`, `last_active`) VALUES
(61, 13, 'dinhtan89', '192.168.60.10', '2016-05-17 13:56:43', '0000-00-00 00:00:00'),
(67, 13, 'dinhtan89', '', '2016-05-27 09:40:48', '0000-00-00 00:00:00'),
(97, 14, 'qthai2502', '192.168.40.13', '2016-06-24 10:09:52', '0000-00-00 00:00:00'),
(101, 16, 'wantlose', '192.168.60.12', '2016-06-27 09:45:12', '0000-00-00 00:00:00'),
(103, 12, 'CyberZ', '192.168.60.10', '2016-07-01 10:02:12', '0000-00-00 00:00:00'),
(105, 17, 'duongdt', '', '2016-07-07 10:08:40', '0000-00-00 00:00:00'),
(107, 14, 'qthai2502', '', '2016-07-08 09:08:06', '0000-00-00 00:00:00'),
(108, 10, 'tranvutuan', '192.168.50.12', '2016-07-08 09:09:59', '0000-00-00 00:00:00'),
(112, 11, 'kimhc0210', '192.168.60.17', '2016-07-28 09:20:11', '0000-00-00 00:00:00'),
(114, 10, 'tranvutuan', '192.168.60.11', '2016-08-01 09:37:26', '0000-00-00 00:00:00'),
(114, 11, 'kimhc0210', '192.168.60.14', '2016-08-01 09:36:36', '0000-00-00 00:00:00'),
(114, 12, 'CyberZ', '192.168.60.12', '2016-08-01 09:36:33', '0000-00-00 00:00:00'),
(114, 16, 'wantlose', '192.168.60.17', '2016-08-01 09:39:06', '0000-00-00 00:00:00'),
(114, 17, 'duongdt', '192.168.60.10', '2016-08-01 09:36:40', '0000-00-00 00:00:00'),
(115, 14, 'qthai2502', '192.168.40.11', '2016-08-01 14:40:58', '0000-00-00 00:00:00');

-- --------------------------------------------------------

--
-- Table structure for table `servers`
--

CREATE TABLE IF NOT EXISTS `servers` (
  `server_id` int(11) NOT NULL,
  `host` varchar(50) CHARACTER SET utf8 NOT NULL,
  `port` int(11) NOT NULL,
  `hub` varchar(255) CHARACTER SET utf8 NOT NULL,
  `create_time` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `number_connected` int(11) NOT NULL DEFAULT '0'
) ENGINE=InnoDB AUTO_INCREMENT=12 DEFAULT CHARSET=latin1;

--
-- Dumping data for table `servers`
--

INSERT INTO `servers` (`server_id`, `host`, `port`, `hub`, `create_time`, `number_connected`) VALUES
(2, '103.56.157.252', 992, 'aoe_vpn_hub', '2016-01-25 17:14:46', 1),
(3, '103.56.157.252', 5556, 'aoe_vpn_hub2', '2016-01-25 17:14:46', 1),
(4, '103.56.157.252', 443, 'aoe_vpn_hub3', '2016-01-25 17:16:11', 1),
(5, '103.56.157.252', 5555, 'aoe_vpn_hub4', '2016-01-25 17:16:11', 0),
(6, '103.56.157.252', 5556, 'aoe_vpn_hub5', '2016-01-25 17:16:11', 0),
(7, '103.56.157.252', 5557, 'aoe_vpn_hub6', '2016-01-25 17:16:11', 0),
(8, '103.56.157.252', 5558, 'aoe_vpn_hub7', '2016-01-25 17:16:11', 0),
(9, '103.56.157.252', 5559, 'aoe_vpn_hub8', '2016-01-25 17:16:11', 0),
(10, '103.56.157.252', 5560, 'aoe_vpn_hub9', '2016-01-25 17:16:11', 0),
(11, '103.56.157.252', 5561, 'aoe_vpn_hub10', '2016-01-25 17:16:11', 0);

-- --------------------------------------------------------

--
-- Table structure for table `user_caches`
--

CREATE TABLE IF NOT EXISTS `user_caches` (
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
) ENGINE=InnoDB AUTO_INCREMENT=22 DEFAULT CHARSET=latin1;

--
-- Dumping data for table `user_caches`
--

INSERT INTO `user_caches` (`user_id`, `user_name`, `user_email`, `password`, `last_active`, `avatar`, `level`, `diamond`, `status`, `state`) VALUES
(9, 'admintest1', '', 'Admin12345678', '2016-08-16 08:36:36', '', 0, 0, 'Lan dau choi game', 1),
(10, 'tranvutuan', '', 'Tuan@123', '2016-08-04 12:34:16', '', 0, 0, 'yeu doi vai luong', 1),
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
(21, 'nguycongtu', '', 'Vxn6bbx5', '2016-08-01 10:10:13', '', 0, 0, '', 1);

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
  ADD PRIMARY KEY (`room_id`,`user_id`), ADD UNIQUE KEY `room_id` (`room_id`,`user_id`);

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
  MODIFY `message_id` int(11) NOT NULL AUTO_INCREMENT,AUTO_INCREMENT=9;
--
-- AUTO_INCREMENT for table `rooms`
--
ALTER TABLE `rooms`
  MODIFY `room_id` int(11) NOT NULL AUTO_INCREMENT,AUTO_INCREMENT=122;
--
-- AUTO_INCREMENT for table `servers`
--
ALTER TABLE `servers`
  MODIFY `server_id` int(11) NOT NULL AUTO_INCREMENT,AUTO_INCREMENT=12;
--
-- AUTO_INCREMENT for table `user_caches`
--
ALTER TABLE `user_caches`
  MODIFY `user_id` int(11) NOT NULL AUTO_INCREMENT,AUTO_INCREMENT=22;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
