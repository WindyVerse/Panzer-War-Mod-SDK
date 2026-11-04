Behavior()
Property = {}
local GetVehicleHP = class("GetVehicleHP")

function GetVehicleHP:OnStarted()
    self.OnTotalDamageChanged = function(oldVal, newVal)
        print(tostring(oldVal) .. " -> " .. tostring(newVal))
    end

    self.vehicle.basePawn.totalDamage:OnValueChanged("+", self.OnTotalDamageChanged)
end

function GetVehicleHP:OnUpdated()
    local hp = self.vehicle.basePawn.hp:Get()
    -- print(hp)

    local totalDamage = self.vehicle.basePawn.totalDamage:Get()
    -- print(totalDamage)
end

return GetVehicleHP
