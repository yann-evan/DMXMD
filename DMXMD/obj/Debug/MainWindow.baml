   M S B A M L   `   `   `  ����@  <DMXMD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:WpfApplication2WpfApplication2  5   	   L HDmxUserControlLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullP:clr-namespace:DmxUserControlLib;assembly=DmxUserControlLibDmxUserControlLib 5   	        WpfApplication2.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   /localclr-namespace:WpfApplication2     5   	   XDmxUserControlLib:clr-namespace:DmxUserControlLib;assembly=DmxUserControlLib     5   	   -   5          � Title$  DMXMD��5	   	   $	��500��6   $
��1100��6$    � 
ResizeMode$ NoResize=�6J    � WindowStartupLocation$ CenterScreen=�6`   .��5
      � $��5q�6    � RowDefinitions 5   
   �� 5      $
��auto �6   �� 5      $
��auto �6    � ColumnDefinitions 5   
   �� 5      $
��auto �6   �� 5      $
��auto �6   .+�5   
   '  !DmxUserControlLib.MatriceLedAPC40  -   5          Name$ 
MatriceLed��5   ,   $��0��6@   $��0��6P   $	��400��6^   $	��800��6k   $  DmxUserControlLib.APC40LedConf  5   
   -   5          Name$ 	ConfigLed��5   )   $��1��6<   $��0��6L   6Y   � 5   
   $��0��6   $��2��6    5      �� 5      $
��auto �6#   �� 5      $
��auto �6#   �� 5      $
��auto �6#   �� 5      $
��auto �6#   .+�5      �� -   5         ��Name$ MapName_TextBox��5      $��0��6/   $��70��6?   $��10q�6J   �� 5       -   5         ��Name$ 
New_Button��5       $��1��6)   $��70��69   $	��New��6D   $��10q�6R   6^   �� 5!      -   5        $ Open_button��5!      $��2��6*   $��70��6:   $
��Open��6E   $��10q�6T   6`   �� 5"      -   5        $ Save_Button��5"      $��3��6*   $��70��6:   $
��Save��6E   $��10q�6T   6`   5$      5&      5'      