package groups.workoutPermission.permission;

import org.springframework.http.HttpStatus;

import java.util.UUID;

public interface PermissionValidator {

    HttpStatus checkPermissionId(UUID permissionId);
}
