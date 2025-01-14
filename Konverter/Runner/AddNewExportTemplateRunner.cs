﻿// Konverter - Plain text file style converter
// Copyright (C) 2021 Witches Banquet
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License,
// or any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY

namespace Konverter.Runner;

public class AddNewExportTemplateRunner : IRunner
{
    private readonly ILogger<AddNewExportTemplateRunner> _logger;
    private readonly ITemplateService _templateService;

    public AddNewExportTemplateRunner(ILogger<AddNewExportTemplateRunner> logger, ITemplateService templateService)
    {
        _logger = logger;
        _templateService = templateService;
    }

    public RunnerType GetRunnerType()
    {
        return RunnerType.AddNewExportTemplateRunner;
    }

    public Task Run()
    {
        _logger.LogInformation("Start to add new export template");

        // Get files
        var fileCheck = false;
        var indexCheck = false;
        var filePath = "";
        var indexPath = "";
        while (fileCheck is false)
        {
            filePath = AnsiConsole.Ask<string>("请输入 模版文件路径 (输入 %quit 退出)：");
            if (filePath.Trim().ToLower() == "%quit")
            {
                _logger.LogInformation("Import template operation canceled by user");
                return Task.CompletedTask;
            }

            if (File.Exists(filePath) is false)
            {
                continue;
            }

            _logger.LogInformation("File path is valid, export template original file path is {path}", filePath);
            fileCheck = true;
        }
        while (indexCheck is false)
        {
            indexPath = AnsiConsole.Ask<string>("请输入 目录模版文件路径 (输入 %quit 退出，即无 Index 模版)：");
            if (indexPath.Trim().ToLower() == "%quit")
            {
                _logger.LogInformation("No index template");
                break;
            }

            if (File.Exists(indexPath) is false)
            {
                continue;
            }

            _logger.LogInformation("File path is valid, export index template original file path is {path}", filePath);
            indexCheck = true;
        }

        // Filename template
        var ft = AnsiConsole.Ask<string>("请输入 文件名模版 (输入 %quit 退出，即不从文件名读取信息)：");
        if (ft.Trim().ToLower() == "%quit")
        {
            _logger.LogInformation("No filename template");
            ft = "";
        }

        // Metadata
        var name = AnsiConsole.Ask<string>("请输入 模版名称 (输入 %quit 退出)：");
        if (name.Trim().ToLower() == "%quit")
        {
            _logger.LogInformation("Import template operation canceled by user");
            return Task.CompletedTask;
        }
        var description = AnsiConsole.Ask<string>("请输入 模版描述 (输入 %quit 退出)：");
        if (description.Trim().ToLower() == "%quit")
        {
            _logger.LogInformation("Import template operation canceled by user");
            return Task.CompletedTask;
        }
        var fe = AnsiConsole.Ask<string>("请输入 输出文件后缀 (输入 %quit 退出)：");
        if (fe.Trim().ToLower() == "%quit")
        {
            _logger.LogInformation("Import template operation canceled by user");
            return Task.CompletedTask;
        }

        // Build model and add
        var model = new ExportTemplate { Name = name, Description = description, FileNameTemplate = ft, FileExtension = fe.Trim().ToLower() };
        var obj = _templateService.ExportTemplateFromFile(filePath, indexPath, model);

        // Finish check
        if (obj is null)
        {
            _logger.LogError("Failed to add new export template");
            AnsiConsole.Write(new Markup("[red]添加失败，请查看日志文件获取详情[/]"));
            AnsiConsole.WriteLine();
            return Task.CompletedTask;
        }
        _logger.LogInformation("Add export template successfully, id is {id}", obj.Id);
        AnsiConsole.Write(new Markup($"[green]添加成功，模版 Id：{obj.Id}[/]"));
        AnsiConsole.WriteLine();
        return Task.CompletedTask;
    }
}
