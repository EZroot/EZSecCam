   M S B A M L   `   `   `  ����C  ?EZSecCam, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:EZSecCamEZSecCam  5   	       EZSecCam.ConnectionWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   (localclr-namespace:EZSecCam     5   	     � Title$  ConnectionWindow��5   	   $	��270��6"   $	��420��6/    � WindowStartupLocation$ CenterScreen=�6;   .��5	      �  � ColumnDefinitions 5
   
   �� 5      $��2* �6   �� 5      $��1* �6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $��* �6   .+�5   
   �� $��15q�6   $4��.Connect to a server IP to remote view a camera��6    $��0��6W   $��0��6g   $
��Left=�6t   $��Center=�6�   �� 5   
   $��	Server IP��6   $��0��6&   $��1��66   $��Center=�6C   $	��Top=�6`   �� 5   
   -   5         ��Name$ IPInputTextBox��5   �   $��15q�6   $��17��6   $��0��6.   $��1��6>   $��Stretch=�6K   $	��Top=�6i   6�   �� 5   
   $��
Public Key��6   $��0��6'   $��2��67   $��Center=�6D   $	��Top=�6a   �� 5   
   -   5        $ PublicInputTextBox��5   �   $��15q�6   $��17��6   $��0��6.   $��2��6>   $��Stretch=�6K   $	��Top=�6i   6�   �� 5   
   $��
Public Exp��6   $��1��6'   $��2��67   $��Center=�6D   $	��Top=�6a   �� 5   
   -   5        $  PublicExponentInputTextBox��5   �   $��15q�6   $��65537��6   $��17��6+   $��1��6:   $��2��6J   $��Stretch=�6W   $	��Top=�6u   6�   �� 5   
   -   5         ��Name$ ApplySettingsButton��5      $��Apply��6.   $��1��6>   $��5��6N   $��Bottom=�6[   $��Stretch=�6v   $��2q�6�   6�   �� 5   
   -   5        $ ConnectSettingsButton��5      $��Connect to Server��60   $��0��6L   $��5��6\   $��Bottom=�6i   $��Stretch=�6�   $��2q�6�   6�   5      5      