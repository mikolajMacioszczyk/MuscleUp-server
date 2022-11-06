package groups.group.controller.form;

public record GroupForm(
        String name,
        String description,
        boolean repeatable
) { }
